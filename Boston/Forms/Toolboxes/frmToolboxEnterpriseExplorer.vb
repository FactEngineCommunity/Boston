Imports System.IO
Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Runtime
Imports Gios.Word
Imports System.Xml.Schema

Imports <xmlns:ns="http://www.w3.org/2001/XMLSchema">
Imports <xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore">
Imports <xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram">
Imports VDS.RDF
Imports VDS.RDF.Parsing
Imports VDS.RDF.Query
Imports VDS.RDF.Query.Patterns

Public Class frmToolboxEnterpriseExplorer

    Public zoRecentNodes As tRecentlyViewedNodes
    Public zsRecentNodesFileName As String
    Dim ziCurrentSearchItem As Integer = 0

    Private WithEvents zrToolTip As New ToolTip

    Private zbSetupFormComplete As Boolean = False
    Private zoCurrentNode As TreeNode = Nothing

    Private ziSelectedProjectIndex As Integer = -1

    Public zbLoadingProjects As Boolean = False
    Private zbRemovingModels As Boolean = False

    Private zrProject As ClientServer.Project
    Private zsNamespaceId As String
    Private zbDraggingOver As Boolean = False
    Private ziMouseButton As MouseButtons


    ''' <summary>
    ''' NB SetupForm called from Timer_FormSetup.Tick
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub frm_enterprise_tree_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Me.Visible = False
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            'Me.ToolStripMenuItemExportToNORMAormFile.Enabled = My.Settings.SuperuserMode

            Me.CircularProgressBar.Value = 0

            '==========================================================================================
            '---------------------------------------------------------------------------------------            
            'Limit functionality depending on the Boston SoftwareCategory
            Me.ToolStripMenuItemModelConfiguration.Enabled = _
            (prApplication.SoftwareCategory = pcenumSoftwareCategory.Professional)

            '=========================================================================================

            frmMain.ToolStripMenuItemRecentNodes.Enabled = True

            '------------------------------------------------
            'SetupForm
            'Timer_FormSetup kicks in after 100 milliseconds
            '------------------------------------------------
            Me.TreeView.HideSelection = False

            zoRecentNodes = New tRecentlyViewedNodes

            Dim lsProgrammeDataPath As String = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData

            zsRecentNodesFileName = lsProgrammeDataPath & "\" & My.Settings.RecentFilesName

            If File.Exists(zsRecentNodesFileName) Then
                zoRecentNodes.Deserialize(zsRecentNodesFileName)
                Call Me.UpdateRecentNodesMenu()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Called from Timer
    ''' </summary>
    Sub SetupForm()

        Dim loNode As TreeNode

        Try
            loNode = Me.TreeView.Nodes.Add("Models", "Models", 0, 0)

            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.menuBoston,
                                                       Nothing,
                                                       Nothing,
                                                       Nothing,
                                                       loNode)

#Region "Client/Server"
            Me.ToolStripMenuItemMoveModel.Visible = My.Settings.UseClientServer
            If My.Settings.UseClientServer Then
                Me.ToolStripMenuItemCodeGenerator.Enabled = My.Settings.ClientServerViewCodeGenerator
                Me.LabelPromptProject.Visible = True
                Me.ComboBoxProject.Visible = True
                Me.LabelPromptNamespace.Visible = True
                Me.ComboBoxNamespace.Visible = True
                Call Me.LoadProjects()
                If Me.ComboBoxProject.Items.Count > 0 Then
                    Dim lrProject As ClientServer.Project = Me.ComboBoxProject.SelectedItem.Tag
                    Call Me.loadNamespacesForProject(lrProject)
                End If
            Else
                Call Me.LoadModels(Nothing)
                Me.LabelPromptProject.Visible = False
                Me.ComboBoxProject.Visible = False
            End If
#End Region

            Call LoadEnterpriseTreeSearchItems()

            Call frmMain.ShowHideMenuOptions()
            Call Me.ShowHideMenuItems()

            Me.TreeView.Nodes(0).Expand()

            If Me.TreeView.Nodes(0).Nodes.Count = 1 Then
                Me.TreeView.Nodes(0).Nodes(0).Expand()
            End If

            Me.zbSetupFormComplete = True

            Me.ToolStripMenuItemModelConfiguration.Visible = My.Settings.AllowModelConfiguration

            '--------------------------------------------------------------------------------------
            'If the User double-clicked on a .fbm file to start Boston, psStartupFBMFile is populated.
            Call Me.loadFBMXMLFile2(psStartupFBMFile)
            psStartupFBMFile = ""

            Me.LabelHelpTips.Text = "Right click on [Models] to add a new model."

            If My.Settings.UseClientServer Then
                Call Me.loadUserToJoinProjectInvitations()
                Call Me.loadUserToJoinGroupInvitations()
            End If

            Cursor.Current = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadModels(Optional ByVal asCreatedByUserId As String = Nothing,
                           Optional ByVal asNamespaceId As String = Nothing)


        Try
            Dim liModelCount As Integer = 0
            Dim lrModel As FBM.Model
            Dim larModel As New List(Of FBM.Model)

            larModel = TableModel.GetModels(asCreatedByUserId, asNamespaceId)
            prApplication.Models = larModel

            Dim lasExcludedModelIds As New List(Of String)

            If Not My.Settings.DisplayLanguageModel Then
                lasExcludedModelIds.Add("English")
            End If

            If Not My.Settings.DisplayCoreMetaModel Then
                lasExcludedModelIds.Add("Core")
            End If

            For Each lrModel In larModel
                If Not lasExcludedModelIds.Contains(lrModel.ModelId) Then
                    '------------------------------------------------------------------
                    'Limit the number of Models the user can work with to 3 Models if
                    '  they are using Boston Student
                    '------------------------------------------------------------------
                    If (prApplication.SoftwareCategory = pcenumSoftwareCategory.Student) And (liModelCount = 10) Then
                        Exit For
                    End If

                    If lrModel.ModelId = "Core" Then
                        lrModel = prApplication.CMML.Core
                    End If

                    'Namespace
                    If prApplication.WorkingProject Is Nothing Then prApplication.WorkingProject = New ClientServer.Project("MyPersonalModels", "MyPersonalModels")
                    If prApplication.WorkingProject.Id = "MyPersonalModels" Then
                        lrModel.Namespace = Nothing
                    Else
                        lrModel.Namespace = prApplication.WorkingNamespace
                    End If

                    Call Me.AddModelToModelExplorer(lrModel, True)
                    liModelCount += 1

                    prApplication.Models.AddUnique(lrModel)
                End If
            Next

            Me.TreeView.Nodes(0).Expand()
            Me.TreeView.Nodes(0).EnsureVisible()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub AddExistingModel(ByRef arModel As FBM.Model)

        Call Me.AddModelToModelExplorer(arModel, True)

    End Sub

    Public Function AddModelToModelExplorer(ByRef arModel As FBM.Model, Optional ByVal abLoadPages As Boolean = False) As TreeNode

        Dim loNode As TreeNode

        loNode = Me.TreeView.Nodes("Models").Nodes.Add(arModel.ModelId, arModel.Name, 1, 1)
        loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.modelORMModel,
                                                   Nothing,
                                                   arModel.ModelId,
                                                   pcenumLanguage.ORMModel,
                                                   loNode)

        If My.Settings.FactEngineShowDatabaseLogoInModelExplorer Then
            Select Case arModel.TargetDatabaseType
                Case Is = pcenumDatabaseType.MongoDB
                    loNode.ImageIndex = 6
                    loNode.SelectedImageIndex = 6
                Case Is = pcenumDatabaseType.SQLServer
                    loNode.ImageIndex = 9
                    loNode.SelectedImageIndex = 9
                Case Is = pcenumDatabaseType.MSJet
                    loNode.ImageIndex = 7
                    loNode.SelectedImageIndex = 7
                Case Is = pcenumDatabaseType.SQLite
                    loNode.ImageIndex = 8
                    loNode.SelectedImageIndex = 8
                Case Is = pcenumDatabaseType.ODBC
                    loNode.ImageIndex = 10
                    loNode.SelectedImageIndex = 10
                Case Is = pcenumDatabaseType.PostgreSQL
                    loNode.ImageIndex = 11
                    loNode.SelectedImageIndex = 11
                Case Is = pcenumDatabaseType.Snowflake
                    loNode.ImageIndex = 12
                    loNode.SelectedImageIndex = 12
                Case Is = pcenumDatabaseType.TypeDB
                    loNode.ImageIndex = 13
                    loNode.SelectedImageIndex = 13
                Case Is = pcenumDatabaseType.Neo4j
                    loNode.ImageIndex = 14
                    loNode.SelectedImageIndex = 14
                Case Is = pcenumDatabaseType.KuzuDB
                    loNode.ImageIndex = 22
                    loNode.SelectedImageIndex = 22
                Case Is = pcenumDatabaseType.RelationalAI
                    loNode.ImageIndex = 21
                    loNode.SelectedImageIndex = 21
                Case Is = pcenumDatabaseType.EdgeDB
                    loNode.ImageIndex = 23
                    loNode.SelectedImageIndex = 23
                Case Is = pcenumDatabaseType.None
                    If arModel.StoreAsXML Then
                        loNode.ImageIndex = 20
                        loNode.SelectedImageIndex = 20
                    End If
            End Select
        End If

        loNode.Tag.Tag = arModel

        arModel.TreeNode = New TreeNode
        arModel.TreeNode = loNode

        '------------------------------
        'Load the Pages for the Model
        '------------------------------
        If abLoadPages Then
            Call arModel.LoadPages()
        End If

        Call Me.AddPagesForModel(arModel, loNode)

        prApplication.Models.AddUnique(arModel)

        Return loNode

    End Function


    Private Sub frm_enterprise_tree_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        Dim lsProgrammeDataPath As String = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData
        Me.zsRecentNodesFileName = lsProgrammeDataPath & "\" & My.Settings.RecentFilesName

        Call Me.zoRecentNodes.Serialize(Me.zsRecentNodesFileName)

        frmMain.MenuItem_ShowEnterpriseTreeView.Checked = False
        'frmMain.zfrmModelExplorer = Nothing

        '----------------------------------------------
        'Close the ModelDictionary form if it is open
        '----------------------------------------------
        Dim child As New frmToolboxModelDictionary

        child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)

        If prApplication.RightToolboxForms.Count > 0 Then
            If IsSomething(child) Then
                If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                    child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                    child.Close()
                End If
            End If
        End If

        '----------------------------------------------
        'Close the Toolbox form if it is open
        '----------------------------------------------
        Dim lrToolboxFrm As New frmToolbox

        lrToolboxFrm = prApplication.RightToolboxForms.Find(AddressOf lrToolboxFrm.EqualsByName)

        If prApplication.RightToolboxForms.Count > 0 Then
            If IsSomething(child) Then
                If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                    lrToolboxFrm = prApplication.RightToolboxForms.Find(AddressOf lrToolboxFrm.EqualsByName)
                    lrToolboxFrm.Close()
                End If
            End If
        End If

        Dim WeifenLuForm As WeifenLuo.WinFormsUI.Docking.DockContent

        For Each WeifenLuForm In prApplication.RightToolboxForms.ToArray
            WeifenLuForm.Close()
        Next

        For Each WeifenLuForm In prApplication.ToolboxForms.ToArray
            WeifenLuForm.Close()
        Next

        '-------------------------------------------------------------------------------------
        'Ask the user if they want to save the Working Model if it hasn't been saved/isdirty
        '-------------------------------------------------------------------------------------
        If IsSomething(prApplication.WorkingModel) Then
            If prApplication.WorkingModel.IsDirty Then
                Dim lsMessage As String = ""
                lsMessage = "The Model, '" & prApplication.WorkingModel.Name & "', has been modified."
                lsMessage &= vbCrLf & "Would you like to save the changes to the Model?"
                If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    prApplication.WorkingModel.Save()
                End If
            End If
        End If

        frmMain.ToolStripMenuItemRecentNodes.Enabled = False

    End Sub

    Private Sub frmToolboxEnterpriseTree_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing


        Using loWaitCursor As New WaitCursor

            Try
                Dim lrEnterpriseView As New tEnterpriseEnterpriseView
                Dim lrPage As FBM.Page

                For Each lrEnterpriseView In prPageNodes
                    lrPage = lrEnterpriseView.Tag
                    If IsSomething(lrPage.Form) Then
                        lrPage.Form.Close()
                    End If
                Next

                If IsSomething(frmMain.zfrmStartup) Then
                    frmMain.zfrmStartup.Close()
                End If

                prApplication.WorkingModel = Nothing
                prApplication.WorkingPage = Nothing
                prApplication.WorkingValueType = Nothing
                prApplication.WorkingProject = Nothing
                prApplication.WorkingNamespace = Nothing

                prApplication.ORMQL = New ORMQL.Processor

                prPageNodes.Clear()
                Dim lasExcludedModels = {"Core", "English"}
                For Each lrModel In prApplication.Models.ToArray
                    If Not lasExcludedModels.Contains(lrModel.ModelId) Then
                        prApplication.Models.Remove(lrModel)
                    End If
                Next

                prApplication.ActivePages.Clear()
                prPageNodes.Clear()
                prApplication.Brain = New tBrain

                frmMain.zfrmModelExplorer = Nothing
                Call frmMain.ShowHideMenuOptions()

                Call frmMain.SetGlobalsToNothing()

                Me.TreeView.Nodes.Clear()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

            Me.zrToolTip.Dispose()

            'See also the Disposed event for this form
            'See also in frmMain
            'Private Sub zfrmModelExplorer_Closed(sender As Object, e As EventArgs) Handles zfrmModelExplorer.Closed

            '    GC.Collect()
            'End Sub

        End Using

    End Sub

    Private Sub LoadEnterpriseTreeSearchItems()

        Dim loWorkingClass As New Object
        Dim llo_strategy_terms As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0

        liReferenceTableId = TableReferenceTable.GetReferenceTableIdByName("EnterpriseTreeSearchItems")

        llo_strategy_terms = TableReferenceFieldValue.GetReferenceFieldValueTuples(liReferenceTableId, loWorkingClass)

        'For liInd = 1 To llo_strategy_terms.Count
        '    If Not IsSomething(My.Settings.EnterpriseTreeSearchList) Then
        '        My.Settings.EnterpriseTreeSearchList = New System.Collections.Specialized.StringCollection
        '    End If
        '    My.Settings.EnterpriseTreeSearchList.Add(llo_strategy_terms(liInd - 1).SearchTerm)
        'Next

    End Sub

    Private Sub UpdateRecentNodesMenu()

        Dim item As ToolStripItem
        Dim text As String
        Dim nodes As List(Of String) = zoRecentNodes.GetNodes()
        Dim counter As Integer = nodes.Count

        frmMain.ToolStripMenuItemRecentNodes.DropDownItems.Clear()

        While counter > 0
            counter = counter - 1
            text = nodes(counter).Replace(CChar(vbTab), " -> ")
            item = frmMain.ToolStripMenuItemRecentNodes.DropDownItems.Add(String.Format("{0}. {1}", nodes.Count - counter, text))
            item.Tag = nodes(counter)
            AddHandler item.Click, AddressOf recentNode_Click
        End While

    End Sub

    Public Sub recentNode_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)
        Dim node As TreeNode = zoRecentNodes.NavigateTo(CStr(item.Tag), Me.TreeView)

        Dim lrMenuOption As tEnterpriseEnterpriseView
        Dim lrPage As FBM.Page = Nothing
        Dim lrModel As FBM.Model = Nothing

        Try
            If node IsNot Nothing Then
                Me.TreeView.Focus()
                node.EnsureVisible()
                Me.TreeView.ForceSelectedNode(node)
                lrMenuOption = Me.TreeView.SelectedNode.Tag

                Select Case lrMenuOption.MenuType
                    Case Is = pcenumMenuType.modelORMModel
                    Case Is = pcenumMenuType.pageORMModel,
                              pcenumMenuType.pageERD
                        lrPage = lrMenuOption.Tag

                        If Not lrPage.Loaded Then
                            frmMain.Cursor = Cursors.WaitCursor
                            frmMain.Invalidate()
                            Call lrPage.Model.Load(True, abDontUseBLOBLoading:=True)
                            frmMain.Cursor = Cursors.Default
                        End If
                End Select
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub RemoveModelFromNode(ByVal arModel As FBM.Model, ByVal aoTreeNode As TreeNode)

        Dim loNode As TreeNode = Nothing
        Dim ls_model_node_prefix As String = ""
        Dim ls_key_prefix As String = ""
        Dim lb_model_type_supported As Boolean = True

        Dim li_richmond_languages() As pcenumLanguage
        Dim li_language As pcenumLanguage


        '------------------------------------------------------------------------
        'Get the list of Languages supported by Richmond from the relevant Enum
        '------------------------------------------------------------------------
        li_richmond_languages = System.Enum.GetValues(GetType(pcenumLanguage))

        For Each li_language In li_richmond_languages
            lb_model_type_supported = True
            Select Case li_language
                Case Is = pcenumLanguage.ORMModel
                    ls_model_node_prefix = "ORM Model"
                    ls_key_prefix = pcenumDiagramType.ORMModel.ToString
                Case Else
                    lb_model_type_supported = False
            End Select

            If lb_model_type_supported Then
                '------------------------------------------------------------
                'Attach the ORM model TreeViewMenuNode and ORM model object
                '------------------------------------------------------------
                loNode = aoTreeNode.Nodes(ls_model_node_prefix).Nodes(arModel.Name)
                loNode.Remove()
            End If
        Next

    End Sub


    Sub AddPagesForModel(ByRef arModel As FBM.Model, ByVal arTreeNode As System.Windows.Forms.TreeNode)

        Dim liPageCount As Integer = 0
        Dim lrPage As FBM.Page

        'Dim lo_parent_node As TreeNode = Nothing 'Used to see what type of page is being added.
        'lo_parent_node = arTreeNode.Parent
        For Each lrPage In arModel.Page
            '-----------------------------------------------------------
            'Check to see that the Language of the Page matches the 
            '  language of the menu item
            '-----------------------------------------------------------
            If lrPage.IsCoreModelPage Then
                '----------------------------------------------------------------------------------------
                'Ignore in the initial load, because CoreMetaModel pages are hidden until elected to be 
                '  displayed by the User (see ContextMenuPage)
                '----------------------------------------------------------------------------------------
            Else
                '-----------------------------------------------------------------------------
                'Limit the number of pages loaded to 10 if the user is using Boston Student.
                '-----------------------------------------------------------------------------
                Call Me.AddExistingPageToModel(lrPage, arModel, arTreeNode)
                liPageCount += 1
            End If
        Next

    End Sub

    Private Function TreeViewRecursiveSearch(ByRef arTreeViewNode As TreeNode, ByRef arObject As Object) As TreeNode

        Try
            For Each lrTreeNode In arTreeViewNode.Nodes
                Select Case arObject.GetType
                    Case Is = GetType(String)
                        If lrTreeNode.Tag.ModelId = arObject Then
                            Return lrTreeNode
                        Else
                            Call Me.TreeViewRecursiveSearch(lrTreeNode, arObject)
                        End If
                    Case Else
                        If lrTreeNode.Tag.ModelId Is arObject Then
                            Return lrTreeNode
                        Else
                            Call Me.TreeViewRecursiveSearch(lrTreeNode, arObject)
                        End If
                End Select

            Next

            Return Nothing

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arPage">The Page to be added to the Model.</param>
    ''' <param name="arModel">The Model that the Page is to be added to.</param>
    ''' <param name="arTreeNode">If Nothing, finds the TreeNode</param>
    ''' <param name="abToolTipNewPage">True if to show a tooltip when the Page is added to the Model in the TreeView.</param>
    ''' <returns></returns>
    Public Function AddExistingPageToModel(ByRef arPage As FBM.Page,
                                      ByRef arModel As FBM.Model,
                                      ByRef arTreeNode As cTreeNode,
                                      Optional ByVal abToolTipNewPage As Boolean = False) As tEnterpriseEnterpriseView

        Try
            Dim loNode As TreeNode
            Dim liPageMenuType As pcenumMenuType = Nothing
            Dim liImageIndex As Integer = pcenumNavigationIcons.iconPage

            If arTreeNode Is Nothing Then

                For Each lrTreeNode In Me.TreeView.Nodes
                    arTreeNode = Me.TreeViewRecursiveSearch(lrTreeNode, arModel.ModelId)
                    If arTreeNode IsNot Nothing Then Exit For
                Next

            End If

            If arTreeNode Is Nothing Then Return Nothing

            'If arTreeNode.Tag.LanguageId = arPage.Language Then
            Select Case arPage.Language 'TreeNode.Tag.LanguageId
                Case Is = pcenumLanguage.ORMModel
                    liPageMenuType = pcenumMenuType.pageORMModel
                    liImageIndex = 2
                Case Is = pcenumLanguage.EntityRelationshipDiagram
                    liPageMenuType = pcenumMenuType.pageERD
                    liImageIndex = 4
                Case Is = pcenumLanguage.PropertyGraphSchema
                    liPageMenuType = pcenumMenuType.pagePGSDiagram
                    liImageIndex = 3
                Case Is = pcenumLanguage.StateTransitionDiagram
                    liPageMenuType = pcenumMenuType.pageSTD
                    liImageIndex = 5
                Case Is = pcenumLanguage.UMLUseCaseDiagram
                    liPageMenuType = pcenumMenuType.pageUMLUseCaseDiagram
                    liImageIndex = 15
                Case Is = pcenumLanguage.BPMNChoreographDiagram
                    liPageMenuType = pcenumMenuType.pageBPMNChoreographyDiagram
                    liImageIndex = 16
                Case Is = pcenumLanguage.BPMNCollaborationDiagram
                    liPageMenuType = pcenumMenuType.pageBPMNCollaborationDiagram
                    liImageIndex = 17
                Case Is = pcenumLanguage.BPMNConversationDiagram
                    liPageMenuType = pcenumMenuType.pageBPMNConversationDiagram
                    liImageIndex = 18
                Case Is = pcenumLanguage.BPMNProcessDigram
                    liPageMenuType = pcenumMenuType.pageBPMNProcessDiagram
                    liImageIndex = 19
            End Select
            loNode = arTreeNode.Nodes.Add(Trim(arPage.PageId), Trim(arPage.Name), liImageIndex, liImageIndex)
            loNode.Tag = New tEnterpriseEnterpriseView(liPageMenuType,
                                                       arPage,
                                                       arModel.ModelId,
                                                       arPage.Language,
                                                       loNode,
                                                       arPage.PageId) 'arTreeNode.Tag.LanguageId
            '----------------------------------------------------------------------------------------
            'Register the Page so that when Morphing from Diagram to Diagram, Language to Language
            '  i.e. 'Walking the model via LS', then the TreeNode within the Tree can be found
            '----------------------------------------------------------------------------------------
            prPageNodes.AddUnique(loNode.Tag)
            'Else
            '------------------------------------------------
            'The Page is still valid but does not belong 
            '  at this node of the EnterpriseTreeView
            '------------------------------------------------
            'End If

            If abToolTipNewPage Then

                'Me.zrToolTip.IsBalloon = True
                Dim lsMessage As String = "New Page added To the Model."
                Me.zrToolTip.IsBalloon = True
                Me.zrToolTip.ToolTipIcon = ToolTipIcon.None
                Me.zrToolTip.Show(lsMessage, Me, loNode.Bounds.X, loNode.Bounds.Y + loNode.Bounds.Height, 4000)
            End If

            Return loNode.Tag

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function


    Private Sub AddModelToNode(ByVal aoTreeNode As TreeNode, ByVal arModel As FBM.Model)

        Dim loNode As TreeNode = Nothing
        Dim liImageIndex As Integer = pcenumNavigationIcons.iconORMDiagram
        Dim liMenuType As pcenumMenuType = pcenumMenuType.modelORMModel
        Dim lsLanguageKeyPrefix As String = ""
        Dim laiRichmondLanguages() As pcenumLanguage
        Dim lrEnterpriseViewObject As tEnterpriseEnterpriseView
        Dim leLanguage As pcenumLanguage
        Dim lbLanguageTypeSupported As Boolean = True
        Dim lrORMModel As FBM.Model

        '------------------------------------------------------------------------
        'Get the list of Languages supported by Richmond from the relevant Enum
        '------------------------------------------------------------------------
        laiRichmondLanguages = System.Enum.GetValues(GetType(pcenumLanguage))

        '------------------------------------------------
        'Get the EnterpriseViewObject from the TreeNode
        '------------------------------------------------
        lrEnterpriseViewObject = aoTreeNode.Tag

        If arModel.Loaded Then
            lrORMModel = arModel
        Else
            lrORMModel = New FBM.Model(pcenumLanguage.ORMModel, Trim(arModel.Name), Trim(arModel.ModelId))
        End If

        '------------------------------
        'Load the Pages for the Model
        '------------------------------
        Call lrORMModel.LoadPages()

        '---------------------------------------------------------
        'Add the model to the list of the Models loaded for the
        '  Richmond application.
        '---------------------------------------------------------
        If Not prApplication.Models.Exists(AddressOf lrORMModel.Equals) Then
            prApplication.Models.Add(lrORMModel)
        End If

        For Each leLanguage In laiRichmondLanguages
            lbLanguageTypeSupported = True

            Select Case leLanguage
                Case Is = pcenumLanguage.ORMModel
                    lsLanguageKeyPrefix = leLanguage.ToString
                Case Else
                    lbLanguageTypeSupported = False
            End Select

            liImageIndex = pcenumNavigationIcons.iconDatabase
            liMenuType = pcenumMenuType.modelORMModel

            If lbLanguageTypeSupported Then
                '------------------------------------------------------------
                'Attach the ORM model TreeViewMenuNode and ORM model object
                '------------------------------------------------------------
                loNode = aoTreeNode.Nodes("Model").Nodes(lsLanguageKeyPrefix).Nodes.Add(arModel.ModelId, arModel.Name, liImageIndex, liImageIndex)
                loNode.Tag = New tEnterpriseEnterpriseView(liMenuType, Nothing, Nothing, pcenumLanguage.ORMModel, loNode)
                loNode.Tag.Tag = lrORMModel
                '----------------------------------
                'Add Pages to the Model (TreeNode)
                '----------------------------------
                If IsSomething(lrORMModel.Page) Then
                    Call Me.AddPagesForModel(lrORMModel, loNode)
                End If
            End If
        Next 'Next Language (ORM, UseCaseDiagram, etc)

    End Sub


    Private Sub TreeView1_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles TreeView.AfterLabelEdit

        Try

            Dim loObject As Object

            If e.Label = "" Then
                Exit Sub
            End If

            '------------------------------------------------------------
            'Enable the Save button on the MainFrm.TaskBar
            '------------------------------------------------------------
            frmMain.ToolStripButton_Save.Enabled = True

            '--------------------------------
            'Update the WorkingEnterpriseId
            '--------------------------------
            If IsSomething(Me.TreeView.SelectedNode) Then
                '------------------------
                'Node has been selected
                '------------------------
                If IsSomething(Me.TreeView.SelectedNode.Tag) Then
                    '-----------------------------
                    'A TreeViewMenu object exists
                    '-----------------------------
                    loObject = Me.TreeView.SelectedNode.Tag
                    Select Case loObject.MenuType
                        Case Is = pcenumMenuType.modelORMModel
                            Dim lrModel As New FBM.Model
                            lrModel = loObject.tag
                            If lrModel.SetName(e.Label) Then
                                loObject.tag = lrModel
                            Else
                                Me.TreeView.SelectedNode.Text = lrModel.Name
                            End If

                        Case Is = pcenumMenuType.pageORMModel,
                                  pcenumMenuType.pageERD,
                                  pcenumMenuType.pagePGSDiagram,
                                  pcenumMenuType.pageSTD,
                                  pcenumMenuType.pageUMLUseCaseDiagram,
                                  pcenumMenuType.pageBPMNChoreographyDiagram,
                                  pcenumMenuType.pageBPMNCollaborationDiagram,
                                  pcenumMenuType.pageBPMNConversationDiagram,
                                  pcenumMenuType.pageBPMNProcessDiagram

                            Dim lrModel As FBM.Model = loObject.Tag.Model
                            If lrModel.Page.Find(Function(x) x.Name = e.Label) IsNot Nothing Then
                                MsgBox("That Page name already exists in the Model.")
                                e.CancelEdit = True
                                Exit Sub
                            End If

                            Dim lr_page As New FBM.Page(loObject.tag.Model)
                            lr_page = loObject.tag
                            lr_page.IsDirty = True
                            If IsSomething(lr_page.ReferencedForm) Then
                                lr_page.ReferencedForm.TabText = e.Label
                                lr_page.ReferencedForm.Invalidate()
                                lr_page.ReferencedForm.Refresh()
                            End If
                            frmMain.ToolStripButton_Save.Enabled = True
                            ' Node.Text
                            Call lr_page.SetName(e.Label)

                    End Select
                End If
            End If

        Catch err As Exception
            MsgBox("TreeView_AfterLabelEdit: " & err.Message)
        End Try

    End Sub

    Private Sub TreeView1_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView.DragEnter

        'See if there is a TreeNode being dragged
        'If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
        '    'TreeNode found allow move effect
        '    e.Effect = DragDropEffects.Move
        'Else
        '    'No TreeNode found, prevent move
        '    e.Effect = DragDropEffects.None
        'End If

    End Sub

    Private Sub TreeView1_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView.ItemDrag

        'Set the drag node and initiate the DragDrop 

        Dim lrEnterpriseView As tEnterpriseEnterpriseView
        Dim loObject As New Object
        Dim loTreeNode As TreeNode

        Try

            loTreeNode = e.Item

            lrEnterpriseView = loTreeNode.Tag

            loObject = lrEnterpriseView.Tag

            If loObject.GetType Is GetType(FBM.Model) Then
                Dim lrModel As FBM.Model

                lrModel = loObject
                If lrModel.Loaded Then
                    '----------------------------------------
                    'That's great. Can drag a Loaded Model.
                    '----------------------------------------
                Else
                    '----------------------------------------------------------------------
                    'Let the user know that the Model needs to be loaded before dragging.
                    '----------------------------------------------------------------------
                    Dim lsMessage As String = ""

                    lsMessage = "The Model, '" & lrModel.Name & "', needs to be loaded before dragging to another location on the Enterprise Tree."
                    lsMessage &= vbCrLf & vbCrLf
                    lsMessage &= "Click [Yes] to load the Model now."

                    If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                        Call lrModel.Load(True, abDontUseBLOBLoading:=True)
                    End If

                    Me.TreeView.SelectedNode = loTreeNode

                End If

            End If

            If Boston.GetAsyncKeyState(Keys.ControlKey) Then
                DoDragDrop(e.Item, DragDropEffects.Copy Or DragDropEffects.Move)
            Else
                DoDragDrop(e.Item, DragDropEffects.Move Or DragDropEffects.Copy)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TreeView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView.MouseDown

        Dim loObject As New Object

        Try
            If IsSomething(Me.TreeView.SelectedNode) Or e.Button = Windows.Forms.MouseButtons.Right Then

                If e.Button = Windows.Forms.MouseButtons.Right Then
                    '---------------------------------
                    'Select the node under the Mouse
                    '---------------------------------
                    'Remove then readd handler. Stops the previous Model from being loaded unintentionally.
                    RemoveHandler Me.TreeView.AfterSelect, AddressOf TreeView1_AfterSelect
                    Me.TreeView.SelectedNode = Me.TreeView.GetNodeAt(e.Location)
                    Call Me.TreeView.ForceSelectedNode(Me.TreeView._SelectedNode)
                    'Readd handler. Stops the previous Model from being loaded unintentionally.
                    AddHandler Me.TreeView.AfterSelect, AddressOf TreeView1_AfterSelect

                    If IsSomething(Me.TreeView.SelectedNode) Then
                        If IsSomething(Me.TreeView.SelectedNode.Tag) Then
                            loObject = Me.TreeView.SelectedNode.Tag

                            '------------------------------------------------------
                            'Establish the WorkingEnvironment for the SelectedNode
                            '------------------------------------------------------
                            Call Me.SetWorkingEnvironmentForObject(loObject)

                            '-----------------------------------------------
                            'Establish the ContextMenu for the SelectedNode
                            '-----------------------------------------------
                            Select Case Me.TreeView.SelectedNode.Tag.MenuType
                                Case Is = pcenumMenuType.menuBoston
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_ORMModels
                                Case Is = pcenumMenuType.modelORMModel
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_ORMModel
                                Case Is = pcenumMenuType.pageORMModel
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pageERD
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pagePGSDiagram
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pageSTD
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pageUMLUseCaseDiagram
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pageBPMNChoreographyDiagram
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pageBPMNCollaborationDiagram
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pageBPMNConversationDiagram
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Is = pcenumMenuType.pageBPMNProcessDiagram
                                    Me.TreeView.ContextMenuStrip = ContextMenuStrip_Page
                                Case Else
                                    Me.TreeView.ContextMenuStrip = Nothing
                            End Select
                        Else
                            Me.TreeView.ContextMenuStrip = Nothing
                        End If
                    Else
                        Me.TreeView.ContextMenuStrip = Nothing
                    End If

                ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                    Me.TreeView.SelectedNode = Me.TreeView.GetNodeAt(e.Location)
                    If IsSomething(Me.TreeView.SelectedNode) Then
                        If IsSomething(Me.TreeView.SelectedNode.Tag) Then
                            loObject = Me.TreeView.SelectedNode.Tag

                            '------------------------------------------------------
                            'Establish the WorkingEnvironment for the SelectedNode
                            '------------------------------------------------------
                            With New WaitCursor
                                Call Me.SetWorkingEnvironmentForObject(loObject)
                            End With
                        End If
                    End If
                    '-------------------
                    'See 'AfterSelect'
                    '-------------------
                End If
            End If

        Catch err As Exception
            MsgBox("TreeView_MouseDown: " & err.Message)
        End Try

    End Sub

    Private Sub Timer_FormSetup_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer_FormSetup.Tick

        Me.Timer_FormSetup.Enabled = False
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Call SetupForm()

        Windows.Forms.Cursor.Current = Cursors.Default
        Me.Visible = True

        Call frmMain.resizeModelExplorer()

        Me.Timer_FormSetup.Enabled = False

    End Sub

    Private Sub ShowCircularProgressBar()
        Try
            Me.CircularProgressBar.Top = Me.Height / 3 - (Me.CircularProgressBar.Height / 2)
            Me.CircularProgressBar.Left = Me.Panel1.Width - (Me.CircularProgressBar.Width + 30)
            Me.CircularProgressBar.BringToFront()
            Me.CircularProgressBar.Value = 0
            Me.CircularProgressBar.Value = 1
            Me.CircularProgressBar.Invalidate()
            Me.Invalidate()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub ShowHideMenuItems()

        Try
            If My.Settings.UseClientServer Then
                Me.ToolStripMenuItemKeywordExtractionTool.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub HideCircularProgressBar()

        Try
            Me.CircularProgressBar.Value = 0
            Me.CircularProgressBar.Text = "0%"
            Me.CircularProgressBar.Invalidate()
            Me.CircularProgressBar.SendToBack()
            Me.Invalidate(True)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub DoModelLoading(ByRef arModel As FBM.Model)

        Try
            '==============================================================================================
            'Client/Server
            'The Model hasn't been loaded, but another User may be working on the same Model. So we need
            '  to save the Model (in the other instance) before loading it (in this instance). The reason
            '  is that the other User may not have saved their changes, so the database Model and their 
            '  instance Model will be out of synch.
            If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                lrInterfaceModel.ModelId = arModel.ModelId
                lrInterfaceModel.Name = arModel.Name
                If Not (arModel.ProjectId = "MyPersonalModels" Or arModel.ProjectId = "") Then
                    lrInterfaceModel.ProjectId = arModel.ProjectId
                    If arModel.Namespace IsNot Nothing Then
                        lrInterfaceModel.Namespace = arModel.Namespace.Name
                    End If

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                        Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                        lrBroadcast.Model = lrInterfaceModel
                        Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.SaveModel, lrBroadcast)
                    End If
                End If
            End If


            '==============================================================================================

            '------------------------------
            'Load the Model and the Pages
            '------------------------------                                
            If TableModel.ExistsModelById(arModel.ModelId) And Not arModel.Loaded Then
                Call TableModel.GetModelDetails(arModel)

                Me.Cursor = Cursors.WaitCursor
                Boston.WriteToStatusBar("Loading Model")
                Call Me.ShowCircularProgressBar()
                If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                    pdbConnection.Close() 'keep this here (Close/Open database). Because Access doesn't refresh quick enough from the Save Broadcast above.
                    pdb_OLEDB_connection.Close() 'keep this here (Close/Open database). Because Access doesn't refresh quick enough from the Save Broadcast above.
                    Boston.OpenDatabase() 'keep this here (Close/Open database). Because Access doesn't refresh quick enough from the Save Broadcast above.
                End If

                Dim lrReturnModel = arModel.Load(True, My.Settings.ModelLoadPagesUseThreading, Me.BackgroundWorkerModelLoader)

                If lrReturnModel IsNot arModel Then
                    arModel = lrReturnModel
                End If

                Call Me.HideCircularProgressBar()
                Me.Cursor = Cursors.Default
                Boston.WriteToStatusBar("Loaded Model: '" & arModel.Name & "'")
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView.AfterSelect

        Dim loObject As Object
        Dim lsMessage As String

        Try
            If Me.zbRemovingModels Or Me.zbDraggingOver Then
                Exit Sub
            End If

            '------------------------------
            'Safety checking
            '------------------------------
            If Not e.Node.IsSelected Then
                Exit Sub
            End If

            '------------------------------------------------
            'Set the modal CurrentNode to the selected Node
            '------------------------------------------------
            Me.zoCurrentNode = e.Node

            '----------------------------------
            'Update the recently viewed nodes
            '----------------------------------
            zoRecentNodes.AddNode(e.Node)
            Call Me.UpdateRecentNodesMenu()

            If Me.TreeView.SelectedNode.Text = "Models" Then
                Me.LabelHelpTips.Text = "Right click on [Models] to add a new Model."
            End If

            '--------------------------------
            'Update the WorkingEnterpriseId
            '--------------------------------
            If IsSomething(Me.TreeView.SelectedNode) Then
                '------------------------
                'Node has been selected
                '------------------------
                If IsSomething(Me.TreeView.SelectedNode.Tag) Then
                    '-----------------------------
                    'A TreeViewMenu object exists
                    '-----------------------------
                    loObject = Me.TreeView.SelectedNode.Tag

                    '-----------------------------------------------
                    'Establish the ContextMenu for the SelectedNode
                    '-----------------------------------------------
                    Select Case loObject.MenuType
                        Case Is = pcenumMenuType.modelORMModel
#Region "Model"
                            Dim lrModel As FBM.Model

                            frmMain.ToolStripButtonNew.Enabled = True
                            frmMain.ToolStripButtonPrint.Enabled = False

                            lrModel = loObject.tag

                            '=================================================================
                            'Right Mouse Button
                            If (MouseButtons = MouseButtons.Right) Or (Me.ziMouseButton = MouseButtons.Right) Then Exit Sub

                            Me.TreeView.SelectedNode.Expand()
                            Me.Invalidate()
                            Me.Refresh()
                            frmMain.Refresh()

                            If lrModel.Loaded Then
                                '-------------------------------------------------------------------------------
                                'The Model is already loaded but may need to load the Page data for each Page.
                                '  'Morphin' requires Page data to be loaded so that links between Pages can
                                '  be found using LiNQ.
                                '  NB The call to GetPagesByModel() will not duplicate reloading of a Page.
                                '-------------------------------------------------------------------------------                                
                                If Not lrModel.LoadedFromXMLFile And Not (lrModel.ModelId = "Core") And Not lrModel.StoreAsXML Then
                                    Call TablePage.GetPagesByModel(lrModel, True)
                                End If
                                If lrModel.Loaded Then
                                    Boston.WriteToStatusBar("Model loaded")
                                End If
                            Else
                                With New WaitCursor
                                    Boston.WriteToStatusBar("Loading Model.", True)
                                    Call Me.DoModelLoading(lrModel)
                                End With

                                If lrModel.LoadedFromBLOB Then
                                    loObject.Tag = lrModel
                                    For Each lrTreeNode In Me.TreeView.SelectedNode.Nodes
                                        Call lrTreeNode.Remove
                                        prPageNodes.Remove(lrTreeNode.Tag)
                                    Next
                                    Call Me.AddPagesForModel(lrModel, Me.TreeView.SelectedNode)
                                End If
                            End If

                            '-----------------------------------------
                            'Load the ModelDictionary for the Model
                            '-----------------------------------------
                            Dim lrToolboxForm As frmToolboxModelDictionary
                            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxModelDictionary.Name)

                            If IsSomething(lrToolboxForm) Then
                                Call lrToolboxForm.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
                            End If

                            frmMain.ToolStripButton_Save.Enabled = lrModel.IsDirty
#End Region

                        Case Is = pcenumMenuType.pageORMModel,
                                  pcenumMenuType.pageERD,
                                  pcenumMenuType.pagePGSDiagram,
                                  pcenumMenuType.pageSTD,
                                  pcenumMenuType.pageUMLUseCaseDiagram,
                                  pcenumMenuType.pageBPMNChoreographyDiagram,
                                  pcenumMenuType.pageBPMNCollaborationDiagram,
                                  pcenumMenuType.pageBPMNConversationDiagram,
                                  pcenumMenuType.pageBPMNProcessDiagram

                            Dim lrPage As FBM.Page
#Region "Page"
                            frmMain.ToolStripButtonNew.Enabled = False

                            lrPage = loObject.tag
                            If lrPage.Loaded Or lrPage.Loading Or lrPage.Model.PagesLoading Then
                                '---------------------------------------------------------
                                'The Page is already loaded or is loading so do nothing.
                                '---------------------------------------------------------
                            Else
                                '----------------
                                'Load the Page
                                '----------------
                                Me.Cursor = Cursors.WaitCursor
                                If lrPage.Model.Loaded Then
                                    'Model is already loaded for Page, so do nothing
                                ElseIf Not lrPage.Loading Then
                                    '----------------------------------------------------------------------------------------------
                                    'Load, because the user may have clicked/selected a Page, before clicking/selecting the Model
                                    '  and the Model for the Page is not already loaded. The Page load would otherwise fail.
                                    '  NB Loads all Pages within the Model so that 'Morphing' works. Morphing from one Page to the 
                                    '  next requires that all Pages within the Model are loaded (uses LiNQ queries).
                                    '----------------------------------------------------------------------------------------------
                                    If Not lrPage.Model.Loading Then
                                        With New WaitCursor
                                            Call Me.ShowCircularProgressBar()
                                            lrPage.Model.Load(True, False, Me.BackgroundWorkerModelLoader)
                                            Call Me.HideCircularProgressBar()
                                            Boston.WriteToStatusBar(lrPage.Model.Name & " - Model Loaded")
                                        End With
                                    End If
                                End If

                                Me.Cursor = Cursors.Default
                            End If

                            lsMessage = "- Double-Click on the Page to edit on the canvas."
                            lsMessage &= vbCrLf & "- Click on the Page to change its name."

                            Me.LabelHelpTips.Text = lsMessage
#End Region
                    End Select

                    Call Me.SetWorkingEnvironmentForObject(loObject)
                End If

            End If

            Call frmMain.ShowHideMenuOptions()

        Catch err As Exception
            Me.Cursor = Cursors.Default
            lsMessage = "Error: frmToolboxEnterpriseTree.TreeView.AfterSelect:"
            lsMessage &= vbCrLf & vbCrLf & err.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, err.StackTrace)
        End Try

    End Sub

    Sub SetWorkingEnvironmentForObject(ByVal ao_object As tEnterpriseEnterpriseView)

        Try

            Select Case ao_object.MenuType

                Case Is = pcenumMenuType.modelORMModel

                    Call prApplication.setWorkingModel(ao_object.Tag)
                    prApplication.PluginInterface.SharedModel = prApplication.WorkingModel.SharedModel
                    prApplication.PluginInterface.SharedModel.ModelId = prApplication.WorkingModel.ModelId

                    '---------------------------------------------------------------
                    ' Load the ORMModel object with the ORM Model from the database
                    '  if it already hasn't been loaded.
                    '---------------------------------------------------------------                                                        
                    If Not prApplication.WorkingModel.Loaded Then
                        'Call prApplication.WorkingModel.Load() '20200718-VM-Was previously not commented out. Commented out so can right-click on a Model to delete it.
                    End If

                    '--------------------------------------------------------------
                    'Check to see if there is a Page in the Clipboard for Pasting
                    '--------------------------------------------------------------
                    Dim lrClipboardPage As New FBM.Page 'Clipbrd.ClipboardPage
                    Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")
                    Try
                        If Clipboard.ContainsData(RichmondPage.Name) Then
                            Dim myRetrievedObject As IDataObject = Clipboard.GetDataObject()
                            lrClipboardPage = CType(myRetrievedObject.GetData(RichmondPage.Name), FBM.Page) ' Clipbrd.ClipboardPage)
                            If IsSomething(lrClipboardPage) Then
                                Me.ToolStripMenuItemPastePage.Enabled = True
                            Else
                                Me.ToolStripMenuItemPastePage.Enabled = False
                            End If
                        End If
                    Catch ex As Exception
                        'Oh well, tried
                    End Try

                Case Is = pcenumMenuType.pageORMModel,
                          pcenumMenuType.pageERD,
                          pcenumMenuType.pagePGSDiagram,
                          pcenumMenuType.pageSTD,
                          pcenumMenuType.pageUMLUseCaseDiagram,
                          pcenumMenuType.pageBPMNChoreographyDiagram,
                          pcenumMenuType.pageBPMNCollaborationDiagram,
                          pcenumMenuType.pageBPMNConversationDiagram,
                          pcenumMenuType.pageBPMNProcessDiagram

                    Dim lr_page As FBM.Page = ao_object.Tag

                    prApplication.WorkingModel = lr_page.Model

                    '---------------------------------------------------------------
                    ' Load the ORMModel object with the ORM Model from the database
                    '  if it already hasn't been loaded.
                    '---------------------------------------------------------------                                                        
                    If Not prApplication.WorkingModel.Loaded Then
                        With New WaitCursor
                            Call Me.ShowCircularProgressBar()
                            Call prApplication.WorkingModel.Load(,, Me.BackgroundWorkerModelLoader)
                            Call Me.HideCircularProgressBar()
                        End With
                    End If

                    prApplication.WorkingPage = prApplication.WorkingModel.Page.Find(AddressOf lr_page.Equals)

                    '-------------------------------------------------------
                    'Check if the page has already been opened for editing
                    '-------------------------------------------------------
                    If IsSomething(prApplication.WorkingPage.Form) Then
                        '-----------------------------------------------
                        'The Page has already been loaded for Editing
                        '  Set the ZOrder of the Form on which the Page
                        '  is loaded to OnTop.
                        '-----------------------------------------------                              
                        prApplication.WorkingPage.Form.BringToFront()
                    End If

            End Select
        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TreeView1_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView.DragOver

        Try
            Me.zbDraggingOver = True

            'Check that there is a TreeNode being dragged 
            If e.Data.GetDataPresent("Boston.cTreeNode", True) Then
            Else
                Exit Sub
            End If


            Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)

            'Get the TreeView raising the event (incase multiple on form)
            Dim selectedTreeview As TreeView = CType(sender, TreeView)

            'As the mouse moves over nodes, provide feedback to the user by highlighting the node that is the 
            'current drop target
            Dim pt As Point = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
            Dim targetNode As TreeNode = selectedTreeview.GetNodeAt(pt)

            '------------------------------------------------------
            'Check that the selected node is not the dropNode and also that it is not a child of the dropNode and 
            'therefore an invalid target
            '------------------------------------------------------        
            If targetNode Is Nothing Then Exit Sub

            Dim lrEnterpriseView As tEnterpriseEnterpriseView = targetNode.Tag

            Do Until targetNode Is Nothing
                If targetNode Is dropNode Then
                    e.Effect = DragDropEffects.None
                    selectedTreeview.SelectedNode = Nothing
                    Exit Sub
                End If

                Select Case lrEnterpriseView.MenuType
                    Case Is = pcenumMenuType.pageORMModel, pcenumMenuType.pageERD, pcenumMenuType.pagePGSDiagram
                        e.Effect = DragDropEffects.None
                        selectedTreeview.SelectedNode = Nothing
                        Exit Sub
                End Select

                targetNode = targetNode.Parent
            Loop

            '-------------------------
            'Get the TargetNode again
            '-------------------------
            targetNode = selectedTreeview.GetNodeAt(pt)

            'See if the targetNode is currently selected, 
            'if so no need to validate again
            If (targetNode Is selectedTreeview.SelectedNode) Then
                '---------------------
                'Don't do anything
                '---------------------
            Else
                'Select the node currently under the cursor
                'MsgBox(Control.ModifierKeys.ToString)            
                selectedTreeview.SelectedNode = targetNode
            End If

            'Currently selected node is a suitable target
            If (Control.ModifierKeys = Keys.Control) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.Move
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub EditPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditPageToolStripMenuItem.Click

        Try
            prApplication.ThrowErrorMessage("[Edit Page] Clicked", pcenumErrorType.Information)

            If Nothing Is Me.TreeView.SelectedNode Then
                Exit Sub
            End If

            Dim lsFocusModelElementId As String = Nothing

            Try
                lsFocusModelElementId = Me.TreeView.SelectedNode.Tag.FocusModelElement.Id
            Catch ex As Exception
                lsFocusModelElementId = Nothing
            End Try

            Call Me.LoadSelectedPage(lsFocusModelElementId)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AddPageToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPageToolStripMenuItem1.Click

        Try
            Dim lsMessage As String = ""

            With New WaitCursor

                Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
                If Not lrModel.Loaded Then
                    Call Me.DoModelLoading(lrModel)
                    Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
                End If

                'Make sure all the Pages for the Model are loaded before adding another page
                While prApplication.WorkingModel.Page.FindAll(Function(x) x.Loading).Count > 0 And prApplication.WorkingModel.Page.FindAll(Function(x) x.Loaded).Count <> prApplication.WorkingModel.Page.Count
                End While

                Dim lrPage As FBM.Page

                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = Me.AddPageToModel(Me.TreeView.SelectedNode)
                lrPage = lrEnterpriseView.Tag

                Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                lrInterfaceModel.ModelId = lrPage.Model.ModelId
                lrInterfaceModel.Name = lrPage.Model.Name
                If Not ((lrPage.Model.ModelId <> "MyPersonalModels") Or (lrPage.Model.ProjectId = "")) Then
                    lrInterfaceModel.ProjectId = lrPage.Model.ProjectId
                    lrInterfaceModel.Namespace = lrPage.Model.Namespace.Name
                End If

                Dim lrInterfacePage As New Viev.FBM.Interface.Page
                lrInterfacePage.Id = lrPage.PageId
                lrInterfacePage.Name = lrPage.Name

                lrInterfaceModel.Page = lrInterfacePage

                If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                    Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                    lrBroadcast.Model = lrInterfaceModel
                    Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.ModelAddPage, lrBroadcast)
                End If

                lrEnterpriseView.TreeNode.EnsureVisible()
                Call Me.TreeView.ForceSelectedNode(lrEnterpriseView.TreeNode)
                Me.TreeView.SelectedNode = lrEnterpriseView.TreeNode
                lrEnterpriseView.TreeNode.BeginEdit()

            End With

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub AddModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim loNode As TreeNode = Nothing
        Dim lr_model As FBM.Model
        Dim li_model_count As Integer = Me.TreeView.SelectedNode.Nodes.Count
        Dim ls_model_name As String

        '-----------------------------------------------
        'Add a Model 
        '-----------------------------------------------

        ls_model_name = "New ORM Model " & (li_model_count + 1).ToString


        lr_model = New FBM.Model(pcenumLanguage.ORMModel, ls_model_name)

        lr_model.IsDirty = True
        lr_model.Save()

        prApplication.WorkingModel = lr_model

        '------------------------------------
        'Add a new TreeNode to the TreeView
        '------------------------------------
        loNode = Me.TreeView.SelectedNode.Nodes.Add(lr_model.ModelId, lr_model.Name, 10, 10)
        'loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.ORMModel, prApplication.WorkingModel, prApplication.WorkingEnterpriseId, prApplication.WorkingSubjectAreaId, prApplication.WorkingProjectId, prApplication.WorkingSolutionId, prApplication.WorkingModel.ModelId)

    End Sub

    Private Sub AddUseCasePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Call Me.AddPageToModel(Me.TreeView.SelectedNode)

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arModelNode"></param>
    ''' <param name="arPage"></param>
    ''' <param name="abLoadPage"></param>
    ''' <param name="abToolTipNewPage"></param>
    ''' <param name="abMakeVisible">Ensure the the Page is visible within the Tree after it has been added to the tree.</param>
    ''' <returns></returns>
    Public Function AddPageToModel(ByRef arModelNode As cTreeNode,
                                   Optional ByRef arPage As FBM.Page = Nothing,
                                   Optional ByVal abLoadPage As Boolean = False,
                                   Optional ByVal abToolTipNewPage As Boolean = False,
                                   Optional ByVal abMakeVisible As Boolean = False,
                                   Optional ByVal abSuppressModelSave As Boolean = False) As tEnterpriseEnterpriseView

        Try
            Dim loNode As cTreeNode = Nothing
            Dim lrPage As FBM.Page
            Dim liPageCount As Integer = arModelNode.Nodes.Count 'Me.TreeView.SelectedNode.Nodes.Count
            Dim lsPageName As String = ""
            Dim liMenuType As pcenumMenuType = Nothing
            Dim lsCorePageName As String = ""

            With New WaitCursor

                '---------------------------------------------------
                'Get the MenuType and LanguageType for the new Page
                '---------------------------------------------------
                Dim liNavigationIcon As pcenumNavigationIcons

                '-----------------------------------------------
                'Add a Page to the currently selected Model
                '-----------------------------------------------
                If IsSomething(arPage) Then
                    '------------------------
                    'Page is already created 
                    '------------------------
                    lrPage = arPage
                Else
                    lsPageName = "New Model Page " & (liPageCount + 1).ToString
                    lrPage = New FBM.Page(prApplication.WorkingModel, Nothing, lsPageName, pcenumLanguage.ORMModel) 'Creates a new page for the model.
                    lrPage.Loaded = True
                End If

                Select Case lrPage.Language
                    Case Is = pcenumLanguage.ORMModel
                        liNavigationIcon = pcenumNavigationIcons.iconPage
                        liMenuType = pcenumMenuType.pageORMModel
                    Case Is = pcenumLanguage.PropertyGraphSchema
                        liNavigationIcon = pcenumNavigationIcons.iconPGSPage
                        liMenuType = pcenumMenuType.pagePGSDiagram
                    Case Is = pcenumLanguage.EntityRelationshipDiagram
                        liNavigationIcon = pcenumNavigationIcons.iconERDPage
                        liMenuType = pcenumMenuType.pageERD
                    Case Is = pcenumLanguage.StateTransitionDiagram
                        liNavigationIcon = pcenumNavigationIcons.iconSTDPage
                        liMenuType = pcenumMenuType.pageSTD
                    Case Is = pcenumLanguage.UMLUseCaseDiagram
                        liNavigationIcon = pcenumNavigationIcons.iconUCDPage
                        liMenuType = pcenumMenuType.pageUMLUseCaseDiagram
                    Case Is = pcenumLanguage.BPMNChoreographDiagram
                        liNavigationIcon = pcenumNavigationIcons.iconBPMNChoreorgraphDiagram
                        liMenuType = pcenumMenuType.pageBPMNChoreographyDiagram
                    Case Is = pcenumLanguage.BPMNCollaborationDiagram
                        liNavigationIcon = pcenumNavigationIcons.iconBPMNCollaborationDiagram
                        liMenuType = pcenumMenuType.pageBPMNCollaborationDiagram
                    Case Is = pcenumLanguage.BPMNConversationDiagram
                        liNavigationIcon = pcenumNavigationIcons.iconBPMNConversationDiagram
                        liMenuType = pcenumMenuType.pageBPMNConversationDiagram
                    Case Is = pcenumLanguage.BPMNProcessDigram
                        liNavigationIcon = pcenumNavigationIcons.iconBPMNProcessDiagram
                        liMenuType = pcenumMenuType.pageBPMNProcessDiagram
                End Select

                Dim lrModel As FBM.Model
                lrModel = arModelNode.Tag.Tag
                lrModel.Page.AddUnique(lrPage)

                prApplication.WorkingPage = lrPage

                If Not abSuppressModelSave Then
                    Boston.WriteToStatusBar("Saving Model: " & lrModel.Name)
                    lrModel.Save()
                    Boston.WriteToStatusBar("Model saved.")
                End If

                '------------------------------------
                'Add a new TreeNode to the TreeView
                '------------------------------------        
                loNode = arModelNode.Nodes.Add(lrPage.PageId, lrPage.Name, liNavigationIcon, liNavigationIcon)

                loNode.Tag = New tEnterpriseEnterpriseView(liMenuType,
                                                 prApplication.WorkingPage,
                                                 prApplication.WorkingModel.ModelId,
                                                 lrPage.Language,
                                                 loNode,
                                                 lrPage.PageId)

                Me.TreeView.SelectedNode = loNode

                '---------------------------------------------------------------------------
                'Dirty the Page, so if the User saves the page it is saved to the database
                '---------------------------------------------------------------------------
                lrPage.IsDirty = True
                frmMain.ToolStripButton_Save.Enabled = True

                '------------------------------------
                'Display the Page if required
                '------------------------------------
                If abLoadPage Then
                    Call load_page(lrPage, Me.TreeView.SelectedNode)
                End If

                '----------------------------------------------------------------------------------------
                'Register the Page so that when Morphing from Diagram to Diagram, Language to Language
                '  i.e. 'Walking the model via LS', then the TreeNode within the Tree can be found
                '----------------------------------------------------------------------------------------
                prPageNodes.AddUnique(loNode.Tag)

                If abMakeVisible Then
                    Call loNode.EnsureVisible()
                End If

                If abToolTipNewPage Then

                    Dim lsMessage As String = "New Page added to the Model."
                    Me.zrToolTip.IsBalloon = True
                    Me.zrToolTip.ToolTipIcon = ToolTipIcon.None
                    Me.zrToolTip.Show(lsMessage, Me, loNode.Bounds.X, loNode.Bounds.Y + loNode.Bounds.Height, 4000)
                End If

            End With

            Return loNode.Tag

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Private Sub load_page(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode)

        prApplication.WorkingPage = arPage

        Select Case prApplication.WorkingPage.Language
            Case Is = pcenumLanguage.ORMModel
                Call frmMain.loadORMModelPage(prApplication.WorkingPage, ao_tree_node)
        End Select


    End Sub

    Private Sub ViewModelDictionaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewModelDictionaryToolStripMenuItem.Click

        Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
        If Not lrModel.Loaded Then
            Call Me.DoModelLoading(lrModel)
            Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
        End If

        Dim lfrmToolboxModelDictionary As frmToolboxModelDictionary = frmMain.LoadToolboxModelDictionary()
        lfrmToolboxModelDictionary.zrORMModel = lrModel
        lfrmToolboxModelDictionary.zrLoadedModel = lrModel

    End Sub

    Private Sub AddPageToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Call Me.AddPageToModel(Me.TreeView.SelectedNode)

    End Sub

    Private Sub DeletePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeletePageToolStripMenuItem.Click

        Dim ls_message As String = ""

        ls_message = "Deleting this Page will permanently remove it from the Model within the database." & vbCrLf & vbCrLf
        ls_message &= "Press [OK] is you want to proceed, or [Cancel]"

        If MsgBox(ls_message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then

            '--------------------------------------
            'Delete the Page from the Database
            '--------------------------------------
            Dim lrPage As FBM.Page = Me.TreeView.SelectedNode.Tag.Tag
            Call lrPage.delete()

            '--------------------------------------
            'Remove the Page from the TreeView
            '--------------------------------------
            Me.TreeView.SelectedNode.Remove()

            '--------------------------------------
            'Remove the Page from the Model
            '--------------------------------------
            If lrPage.Form IsNot Nothing Then
                lrPage.Form.Close
                prApplication.ActivePages.Remove(lrPage.Form)
            End If

            '-------------------------------------------------
            'Undo and Redo logs
            Dim larUserAction = (From UserAction In prApplication.UndoLog
                                 Where UserAction.Page Is lrPage
                                 Select UserAction).ToArray
            For Each lrUserAction In larUserAction
                prApplication.UndoLog.Remove(lrUserAction)
            Next

            larUserAction = (From UserAction In prApplication.RedoLog
                             Where UserAction.Page Is lrPage
                             Select UserAction).ToArray
            For Each lrUserAction In larUserAction
                prApplication.UndoLog.Remove(lrUserAction)
            Next


            lrPage.Model.Page.Remove(lrPage)
            lrPage.Empty()
            lrPage.Dispose()

        End If

    End Sub

    Private Sub ToolStripMenuItemAddModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemAddModel.Click

        With New WaitCursor
            Call Me.addNewModelToBoston()
        End With

    End Sub

    Public Function addNewModelToBoston(Optional ByVal asModelName As String = Nothing,
                                        Optional arCreateDatabaseStatement As FEQL.CREATEDATABASEStatement = Nothing) As FBM.Model

        Try
            Dim lrModel As FBM.Model = Nothing

            With New WaitCursor
                Dim lrNewTreeNode As TreeNode = Nothing

                If Me.TreeView.Nodes(0).Nodes.Count > 0 Then
                    Me.TreeView.Nodes(0).Nodes(Me.TreeView.Nodes(0).Nodes.Count - 1).EnsureVisible()
                End If

                lrModel = Me.AddNewModel(lrNewTreeNode, False, arCreateDatabaseStatement)

                '---------------------------------------------------------------------
                'Abort if a Model is not created.
                'e.g. The Student version only allows 3 Modelsin the Model Explorer.
                If lrModel Is Nothing Then Return Nothing

                '==================================================
                'RDS - Create a CMML Page and then dispose of it.            
                'Inject the Core ERD metamodel into the model
                Dim lrPage As FBM.Page
                Dim lrCorePage As FBM.Page

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Injects the lrCorePage's Model Elements into the Model. No need to do anything more with the lrCorePage at all.

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Injects the lrCorePage's Model Elements into the Model. No need to do anything more with the lrCorePage at all.

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Injects the lrCorePage's Model Elements into the Model. No need to do anything more with the lrCorePage at all.

                'lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreBPMNDiagram.ToString)
                'If lrCorePage Is Nothing Then
                '    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreBPMNDiagram.ToString & "', in the Core Model.")
                'End If
                'lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Injects the lrCorePage's Model Elements into the Model. No need to do anything more with the lrCorePage at all.

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString)
                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
                End If
                lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Injects the lrCorePage's Model Elements into the Model. No need to do anything more with the lrCorePage at all.

                'Set the CoreModel VersionNr of the Model.
                lrModel.CoreVersionNumber = prApplication.CMML.Core.CoreVersionNumber

                lrModel.RDSCreated = True
                '==================================================

                '==============================================================================================
                'Client/Server
                If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                    Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                    lrInterfaceModel.ModelId = lrModel.ModelId
                    lrInterfaceModel.Name = lrModel.Name
                    If Not ((lrModel.ProjectId = "MyPersonalModels") Or (lrModel.Namespace Is Nothing)) Then
                        lrInterfaceModel.ProjectId = lrModel.ProjectId
                        lrInterfaceModel.Namespace = lrModel.Namespace.Name
                        If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                            Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                            lrBroadcast.Model = lrInterfaceModel
                            Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.AddModel, lrBroadcast)
                        End If
                    End If
                End If

                '==============================================================================================

                lrModel.Loaded = True 'Important, otherwise adding the Page will try and reload the model.
                Call Me.AddPageToModel(lrNewTreeNode)
                lrModel.TreeNode.EnsureVisible()

                Dim lsMessage As String = "New Model and Page added."

                Me.zrToolTip.IsBalloon = True
                Me.zrToolTip.ToolTipIcon = ToolTipIcon.None
                Me.zrToolTip.Show(lsMessage, Me, lrNewTreeNode.Bounds.X, lrNewTreeNode.Bounds.Y + lrNewTreeNode.Bounds.Height, 4000)

                Call Me.TreeView.Nodes(0).Expand()

            End With 'WaitCursor

            Return lrModel

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try


    End Function

    Public Function AddNewModel(ByRef arCreatedTreeNode As TreeNode,
                                Optional abAddCoreElements As Boolean = True,
                                Optional arCreateDatabaseStatement As FEQL.CREATEDATABASEStatement = Nothing) As FBM.Model

        Try
            Dim loNode As TreeNode = Nothing
            Dim lrModel As FBM.Model
            Dim liModelCount As Integer = Me.TreeView.Nodes(0).Nodes.Count
            Dim lsModelName As String
            Dim lsMessage As String = ""

            If prApplication.SoftwareCategory = pcenumSoftwareCategory.Student Then
                If Me.TreeView.Nodes(0).Nodes.Count >= 10 Then
                    lsMessage = "Boston Student only supports 10 Models in the Model Explorer."
                    lsMessage &= vbCrLf & vbCrLf
                    lsMessage &= "Upgrade to Boston Professional to work with unlimited Models, or remove some Models from the Model Explorer."
                    MsgBox(lsMessage)

                    Return Nothing
                End If
            End If

            '-----------------------------------------------
            'Add a Model 
            '-----------------------------------------------
            lrModel = New FBM.Model

            If arCreateDatabaseStatement IsNot Nothing Then
                lsModelName = arCreateDatabaseStatement.DATABASENAME
            Else
                lsModelName = "New Model "
            End If

            lsModelName = lrModel.CreateUniqueModelName(lsModelName, 0)

            lrModel = New FBM.Model(pcenumLanguage.ORMModel, lsModelName, False)

            If arCreateDatabaseStatement IsNot Nothing Then
                lrModel.TargetDatabaseType = pcenumDatabaseType.SQLite
                'lrModel.TargetDatabaseType = arCreateDatabaseStatement.DATABASETYPE
            End If

            If My.Settings.UseClientServer Or (prApplication.User IsNot Nothing) Then
                lrModel.CreatedByUserId = prApplication.User.Id
            End If

            If prApplication.WorkingProject Is Nothing Then prApplication.WorkingProject = New ClientServer.Project("MyPersonalModels", "MyPersonalModels")
            lrModel.ProjectId = prApplication.WorkingProject.Id

            'Namespace
            If prApplication.WorkingProject.Id = "MyPersonalModels" Then
                lrModel.Namespace = Nothing
            Else
                lrModel.Namespace = Me.ComboBoxNamespace.SelectedItem.Tag
            End If

            'CMML
            If abAddCoreElements Then
                Call lrModel.AddCoreERDPGSSTMUMLModelElements(Nothing)
            End If

            lrModel.IsDirty = True
            lrModel.Save()

            prApplication.WorkingModel = lrModel

            '------------------------------------
            'Add a new TreeNode to the TreeView
            '------------------------------------
            loNode = Me.TreeView.Nodes(0).Nodes.Add(lrModel.ModelId, lrModel.Name, pcenumNavigationIcons.iconDatabase, pcenumNavigationIcons.iconDatabase)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.modelORMModel,
                                                       prApplication.WorkingModel,
                                                       prApplication.WorkingModel.ModelId)

            If arCreateDatabaseStatement IsNot Nothing Then
                If My.Settings.FactEngineShowDatabaseLogoInModelExplorer Then
                    Select Case lrModel.TargetDatabaseType
                        Case Is = pcenumDatabaseType.MongoDB
                            loNode.ImageIndex = 6
                            loNode.SelectedImageIndex = 6
                        Case Is = pcenumDatabaseType.MSJet
                            loNode.ImageIndex = 7
                            loNode.SelectedImageIndex = 7
                        Case Is = pcenumDatabaseType.SQLite
                            loNode.ImageIndex = 8
                            loNode.SelectedImageIndex = 8
                        Case Is = pcenumDatabaseType.SQLServer
                            loNode.ImageIndex = 9
                            loNode.SelectedImageIndex = 9
                        Case Is = pcenumDatabaseType.ODBC
                            loNode.ImageIndex = 10
                            loNode.SelectedImageIndex = 10
                        Case Is = pcenumDatabaseType.PostgreSQL
                            loNode.ImageIndex = 11
                            loNode.SelectedImageIndex = 11
                        Case Is = pcenumDatabaseType.Snowflake
                            loNode.ImageIndex = 12
                            loNode.SelectedImageIndex = 12
                        Case Is = pcenumDatabaseType.TypeDB
                            loNode.ImageIndex = 13
                            loNode.SelectedImageIndex = 13
                        Case Is = pcenumDatabaseType.RelationalAI
                            loNode.ImageIndex = 21
                            loNode.SelectedImageIndex = 21
                    End Select
                End If
            End If

            Call prApplication.addModel(lrModel)

            lrModel.TreeNode = loNode

            arCreatedTreeNode = loNode

            Return lrModel

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try



    End Function


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Call Me.FindTreeNode(Me.TreeView, Me.SearchTextbox.Textbox.Text)

    End Sub

    Private Sub TextBoxSearch_InitiateSearch() Handles SearchTextbox.InitiateSearch

        'If e.KeyCode = Keys.Down Then
        '    If IsSomething(My.Settings.EnterpriseTreeSearchList) Then
        '        If My.Settings.EnterpriseTreeSearchList.Count > 0 Then
        '            Me.ziCurrentSearchItem += 1
        '            If Me.ziCurrentSearchItem > My.Settings.EnterpriseTreeSearchList.Count Then
        '                Me.ziCurrentSearchItem = 1 'My.Settings.EnterpriseTreeSearchList.Count
        '            End If
        '            If My.Settings.EnterpriseTreeSearchList.Count > 0 Then
        '                Me.SearchTextbox.TextBox.Text = My.Settings.EnterpriseTreeSearchList(Me.ziCurrentSearchItem - 1)
        '            End If
        '        End If
        '    End If
        'ElseIf e.KeyCode = Keys.Up Then
        '    If IsSomething(My.Settings.EnterpriseTreeSearchList) Then
        '        If My.Settings.EnterpriseTreeSearchList.Count > 0 Then
        '            'If Trim(TextBox1.Text) = "" Then
        '            Me.ziCurrentSearchItem -= 1
        '            If Me.ziCurrentSearchItem < 1 Then
        '                Me.ziCurrentSearchItem = 1
        '            End If
        '            If My.Settings.EnterpriseTreeSearchList.Count > 0 Then
        '                Me.SearchTextbox.TextBox.Text = My.Settings.EnterpriseTreeSearchList(Me.ziCurrentSearchItem - 1)
        '            End If
        '        End If
        '    End If
        'ElseIf e.KeyCode = Keys.Enter Then

        Dim lsSearchString As String = Trim(Me.SearchTextbox.TextBox.Text)
            '------------------------------------------------------------------
            'Add the Search String to the EnterpriseTreeViewSearchList Setting
            '------------------------------------------------------------------

            If Not IsSomething(My.Settings.EnterpriseTreeSearchList) Then
                My.Settings.EnterpriseTreeSearchList = New System.Collections.Specialized.StringCollection
                My.Settings.EnterpriseTreeSearchList.Clear()
            End If

            'My.Settings.EnterpriseTreeSearchList.Add(Trim(lsSearchString))
            Dim liInd As Integer = 0
            If My.Settings.EnterpriseTreeSearchList.Count > 0 Then
                For liInd = 9 To 1 Step -1
                    My.Settings.EnterpriseTreeSearchList(liInd) = My.Settings.EnterpriseTreeSearchList(liInd - 1)
                Next
                My.Settings.EnterpriseTreeSearchList(0) = Trim(lsSearchString)
            End If
            Me.ziCurrentSearchItem = 1

            '---------------------------------------------------
            'Make sure that the SearchList doesn't get too big
            '---------------------------------------------------
            If My.Settings.EnterpriseTreeSearchList.Count > 10 Then
                My.Settings.EnterpriseTreeSearchList.RemoveAt(9)
            End If

            Call Me.FindTreeNode(Me.TreeView, lsSearchString)
        'End If '20230311-VM-Was for KeyDown. Can remove if all good.

    End Sub

    Private Sub FindTreeNodeRecursive(ByRef arTreeNode As cTreeNode, ByVal asSearchString As String)

        Dim lrTreeNode As cTreeNode

        For Each lrTreeNode In arTreeNode.Nodes
            If asSearchString Is Nothing Then
                lrTreeNode.BackColor = Color.White
                lrTreeNode.Collapse()
            Else
                If lrTreeNode.Text.Contains(asSearchString) Then
                    lrTreeNode.BackColor = Color.DarkSeaGreen
                    lrTreeNode.EnsureVisible()
                Else
                    lrTreeNode.BackColor = Color.White
                End If
            End If

            Call Me.FindTreeNodeRecursive(lrTreeNode, asSearchString)
        Next

    End Sub


    Private Sub FindTreeNode(ByRef arTreeView As TreeView, ByVal asSearchString As String)

        Dim lrTreeNode As TreeNode

        For Each lrTreeNode In Me.TreeView.Nodes
            Call Me.FindTreeNodeRecursive(lrTreeNode, asSearchString)
        Next

    End Sub

    Private Sub DeleteModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteModelToolStripMenuItem.Click

        Dim liInd As Integer = 0
        Dim lrPage As FBM.Page
        Dim lsMessage As String = ""
        Dim lrModel As FBM.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Tag


            lsMessage = "Are you sure you want to delete the Model, '" & lrModel.Name & "' ?"

            If MsgBox(lsMessage, MsgBoxStyle.Critical + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                With New WaitCursor

                    While (lrModel.Loading And Not lrModel.Loaded) Or lrModel.Page.FindAll(Function(x) x.Loading).Count > 0
                    End While

                    While lrModel.RDSLoading
                    End While

                    Dim lrTempNode As TreeNode = Me.TreeView.SelectedNode

                    Call lrModel.TriggerEventDeleting()

                    Application.DoEvents()

                    '-------------------------------------
                    'Remove all the Pages for the Model.
                    '-------------------------------------
                    Dim liPageCount As Integer = lrModel.Page.Count
                    For liInd = liPageCount To 1 Step -1
                        lrPage = lrModel.Page(liInd - 1)

                        Dim lr_enterprise_view As tEnterpriseEnterpriseView
                        Dim loTreeNode As TreeNode
                        lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                                   lrPage,
                                                                   lrPage.Model.ModelId,
                                                                   lrPage.Language,
                                                                   Nothing, lrPage.PageId)

                        If IsSomething(prPageNodes.Find(AddressOf lr_enterprise_view.Equals)) Then

                            loTreeNode = prPageNodes.Find(AddressOf lr_enterprise_view.Equals).TreeNode

                            If loTreeNode Is Nothing Then
                                Throw New System.Exception("Cannot find TreeNode for Page")
                            End If

                            If IsSomething(lrPage.Form) Then
                                lrPage.IsDirty = False
                                Call lrPage.Form.Close()
                            End If

                            loTreeNode.Remove()

                        End If
                        'lrPage.RemoveFromModel()
                        lrModel.Page.RemoveAt(liInd - 1)
                    Next

                    Application.DoEvents()

                    Try
                        Call lrModel.RemoveFromDatabase()
                    Catch ex As Exception
                        If My.Settings.DatabaseType = pcenumDatabaseType.MSJet.ToString Then

                            Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                            lrSQLConnectionStringBuilder.ConnectionString = My.Settings.DatabaseConnectionString

                            Dim lsDatabaseLocation As String = lrSQLConnectionStringBuilder("Data Source")
                            Dim dbe As New DAO.DBEngine
                            Dim db As DAO.Database
                            db = dbe.OpenDatabase(lsDatabaseLocation)
                            dbe.SetOption(DAO.SetOptionEnum.dbMaxLocksPerFile, 200000)
                            db.Execute("DELETE FROM MetaModelModel WHERE ModelId = '" & lrModel.ModelId & "'")
                        End If
                    End Try

                    prApplication.Models.Remove(lrModel)

                    '=================================
                    'Remove then readd handler. Stops the previous Model from being loaded unintentionally.
                    RemoveHandler Me.TreeView.AfterSelect, AddressOf TreeView1_AfterSelect

                    lrTempNode.Remove()
                    Me.TreeView.SelectedNode = Me.TreeView.Nodes(0)

                    AddHandler Me.TreeView.AfterSelect, AddressOf TreeView1_AfterSelect
                    '=================================

                    Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                    lrInterfaceModel.ModelId = lrModel.ModelId

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                        Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                        lrBroadcast.Model = lrInterfaceModel
                        Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.DeleteModel, lrBroadcast)
                    End If

                End With

            End If


        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Application.UseWaitCursor = False
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Public Sub DeleteModelTreeNode(ByRef arModel As FBM.Model)

        Dim liInd As Integer = 0
        Dim lrPage As FBM.Page
        Dim lsMessage As String = ""

        '---------------------------------------------------
        'Firstly see whether the Model is in the TreeView.
        Dim lrTreeNode As TreeNode
        If Me.TreeView.Nodes.Find(arModel.ModelId, True).Count > 0 Then
            lrTreeNode = Me.TreeView.Nodes.Find(arModel.ModelId, True)(0)
        Else
            'No need to delete any TreeNode, because the Model is not in the TreeView
            Exit Sub
        End If

        Me.TreeView.SelectedNode = lrTreeNode

        '-------------------------------------
        'Remove all the Pages for the Model.
        '-------------------------------------
        Dim liPageCount As Integer = arModel.Page.Count
        For liInd = liPageCount To 1 Step -1
            lrPage = arModel.Page(liInd - 1)

            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            Dim loTreeNode As TreeNode

            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                       lrPage,
                                                       lrPage.Model.ModelId,
                                                       pcenumLanguage.ORMModel,
                                                       Nothing, lrPage.PageId)

            If IsSomething(prPageNodes.Find(AddressOf lr_enterprise_view.Equals)) Then

                loTreeNode = prPageNodes.Find(AddressOf lr_enterprise_view.Equals).TreeNode

                If loTreeNode Is Nothing Then
                    Throw New System.Exception("Cannot find TreeNode for Page")
                End If

                If IsSomething(lrPage.Form) Then
                    Call lrPage.Form.Close()
                End If

                loTreeNode.Remove()

            End If

            lrPage.RemoveFromModel()
        Next

        Dim lrTempNode As TreeNode = Me.TreeView.SelectedNode
        Me.TreeView.SelectedNode = Me.TreeView.Nodes(0)
        lrTempNode.Remove()

    End Sub

    Private Sub EmptyModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemEmptyModel.Click

        Dim liInd As Integer = 0
        Dim lrPage As FBM.Page
        Dim lsMessage As String = ""
        Dim lrModel As FBM.Model
        Dim loTreeNode As New TreeNode

        '-----------------------------------------
        'Get the Model from the selected TreeNode
        '-----------------------------------------
        lrModel = New FBM.Model
        lrModel = Me.TreeView.SelectedNode.Tag.Tag

        lsMessage = "Are you sure?"
        lsMessage &= vbCrLf & vbCrLf & "All Model Objects will be removed from the Model, '" & lrModel.Name & "'. This operation cannot be Undone."

        If MsgBox(lsMessage, MsgBoxStyle.YesNo + MsgBoxStyle.Critical) = MsgBoxResult.Yes Then

            While (lrModel.Loading And Not lrModel.Loaded) Or lrModel.Page.FindAll(Function(x) x.Loading).Count > 0
            End While

            With New WaitCursor

                '------------------------------------------------------------------------------------------------------------
                'Remove all the Pages for the Model.
                '  NB Do this processing here because it is easier to remove the TreeNodes from the TreeView on this form.
                '------------------------------------------------------------------------------------------------------------
                Dim liPageCount As Integer = lrModel.Page.Count

                For liInd = liPageCount To 1 Step -1
                    lrPage = lrModel.Page(liInd - 1)

                    Dim lr_enterprise_view As tEnterpriseEnterpriseView
                    loTreeNode = New TreeNode
                    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               lrPage.Language,
                                                               Nothing, lrPage.PageId)

                    loTreeNode = prPageNodes.Find(AddressOf lr_enterprise_view.Equals).TreeNode

                    If IsSomething(lrPage.Form) Then
                        Call lrPage.Form.Close()
                    End If

                    If IsSomething(loTreeNode) Then
                        loTreeNode.Remove()
                        lrPage.RemoveFromModel()
                        prPageNodes.Remove(prPageNodes.Find(AddressOf lr_enterprise_view.Equals))
                    Else
                        Throw New System.Exception("Cannot find TreeNode for Page")
                    End If
                Next

                Me.TreeView.Refresh()

                Call prApplication.UndoLog.Clear()

                Call lrModel.EmptyModel()

            End With
        End If

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub LoadNORMAXMLFile(ByRef arModel As FBM.Model, ByRef arNORMAXMLDOC As XDocument, ByRef arModelTreeNode As TreeNode)

        Dim NORMAXMLDOC As New XDocument
        Dim lrModel As New FBM.Model
        Dim lrValueType As New FBM.ValueType
        Dim lrFactType As New FBM.FactType
        Dim lrRoleConstraint As New FBM.RoleConstraint
        Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
        Dim lrValueTypeInstance As New FBM.ValueTypeInstance
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
        Dim lrRole As New FBM.Role
        Dim lrPage As New FBM.Page
        Dim liInd As Integer = 0
        Dim lrModelTreeNode As New TreeNode

        NORMAXMLDOC = arNORMAXMLDOC
        lrModel = arModel
        lrModel.IsDirty = True
        lrModelTreeNode = arModelTreeNode

        Call prApplication.setWorkingModel(arModel)

        frmMain.Cursor = Cursors.WaitCursor

        '-------------------------------
        'Get the Pages
        '  Add the Page to the TreeView
        '-------------------------------
        Boston.WriteToStatusBar("Loading Pages")
        Dim loEnumElementQueryResult As IEnumerable(Of XElement)
        loEnumElementQueryResult = From ModelInformation In NORMAXMLDOC.Elements.<ormDiagram:ORMDiagram>
                                   Select ModelInformation
                                   Order By ModelInformation.Attribute("Name").Value

        For Each loElement In loEnumElementQueryResult
            lrPage = New FBM.Page(lrModel, loElement.Attribute("id").Value, loElement.Attribute("Name").Value, pcenumLanguage.ORMModel)

            If lrModel.Page.Exists(AddressOf lrPage.Equals) Then
                lrPage = lrModel.Page.Find(AddressOf lrPage.Equals)
            Else
                lrPage.Loaded = True
                lrPage.IsDirty = True
                Dim lr_enterprise_view As tEnterpriseEnterpriseView

                lr_enterprise_view = Me.AddPageToModel(lrModelTreeNode, lrPage, False, False, False, True)

                prPageNodes.AddUnique(lr_enterprise_view)

            End If
        Next

        Dim lrNORMAFileLoader As New NORMA.NORMAXMLFileLoader(lrModel)

        Call lrNORMAFileLoader.getNORMADataTypes(lrModel)

        '-----------------------------------------
        'Get the DataTypes from the nORMa model
        '-----------------------------------------
        Call lrNORMAFileLoader.LoadDataTypes(NORMAXMLDOC)

        '-----------------------------------------
        'Get the ValueTypes from the nORMa model
        '-----------------------------------------
        Boston.WriteToStatusBar("Loading Value Types",, 10)
        Call lrNORMAFileLoader.LoadValueTypes(lrModel, NORMAXMLDOC)

        '-----------------------------------------
        'Get the EntityTypes from the nORMa model
        '-----------------------------------------
        Boston.WriteToStatusBar("Loading Entity Types",, 20)
        Call lrNORMAFileLoader.LoadEntityTypes(lrModel, NORMAXMLDOC)

        '-------------------------------------------------------
        'SimpleReferenceSchemes
        '-------------------------------------------------------
        Call lrNORMAFileLoader.SetSimpleReferenceSchemes(lrModel, NORMAXMLDOC)

        '----------------------------------------
        'Get the FactTypes from the nORMa model
        '----------------------------------------
        Boston.WriteToStatusBar("Loading Fact Types", True, 30)
        Call lrNORMAFileLoader.LoadFactTypes(lrModel, NORMAXMLDOC)

        '----------------------------------------
        'Set Role.Ids for LinkFactTypes
        '----------------------------------------
        Boston.WriteToStatusBar("Loading Fact Types",, 35)
        Call lrNORMAFileLoader.SetRoleIdsForLinkFactTypes(lrModel, NORMAXMLDOC)

        '-----------------------------------------
        'Get the Internal Uniqueness Constraints
        '-----------------------------------------
        Boston.WriteToStatusBar("Loading Internal Uniqueness Constraints",, 40)
        Call lrNORMAFileLoader.LoadRoleConstraintInternalUniquenessConstraints(lrModel, NORMAXMLDOC, True)

        '-----------------------------------------
        'Get the External Uniqueness Constraints
        '-----------------------------------------
        Boston.WriteToStatusBar("Loading External Uniqueness Constraints",, 50)
        Call lrNORMAFileLoader.LoadRoleConstraintExternalUniquenessConstraints(lrModel, NORMAXMLDOC)

        '-------------------------------------------------------
        'SimpleReferenceSchemes for ObjectifyingEntityTypes
        '-------------------------------------------------------
        Call lrNORMAFileLoader.SetSimpleReferenceSchemesObjectifyingEntityTypes(lrModel, NORMAXMLDOC)

        '----------------------------
        'Get the Subset Constraints
        '----------------------------
        Boston.WriteToStatusBar("Loading Subset Constraints",, 60)
        Call lrNORMAFileLoader.LoadRoleConstraintSubsetConstraints(lrModel, NORMAXMLDOC)

        '--------------------------
        'Get the Ring Constraints
        '--------------------------
        Boston.WriteToStatusBar("Loading Ring Constraints",, 65)
        Call lrNORMAFileLoader.LoadRoleConstraintRingConstraints(lrModel, NORMAXMLDOC)

        Boston.WriteToStatusBar("Loading Exclusion Constraints",, 70)
        Call lrNORMAFileLoader.LoadRoleConstraintExclusionConstraints(lrModel, NORMAXMLDOC)


        Boston.WriteToStatusBar("Loading Inclusive Or Constraints",, 75)
        Call lrNORMAFileLoader.LoadRoleConstraintInclusiveOrConstraints(lrModel, NORMAXMLDOC)

        Boston.WriteToStatusBar("Loading Exclusive Or Constraints",, 80)
        Call lrNORMAFileLoader.LoadRoleConstraintExclusiveOrConstraints(lrModel, NORMAXMLDOC)

        Boston.WriteToStatusBar("Loading Equality Constraints",, 85)
        Call lrNORMAFileLoader.LoadRoleConstraintEqualityConstraints(lrModel, NORMAXMLDOC)

        Boston.WriteToStatusBar("Loading Frequency Constraints",, 90)
        Call lrNORMAFileLoader.LoadRoleConstraintFrequencyConstraints(lrModel, NORMAXMLDOC)

        Boston.WriteToStatusBar("Value Comparison Constraints",, 95)
        Call lrNORMAFileLoader.LoadRoleConstraintValueComparisonConstraints(lrModel, NORMAXMLDOC)

        Boston.WriteToStatusBar("Model Notes",, 97)
        Call lrNORMAFileLoader.LoadModelNotes(lrModel, NORMAXMLDOC)

        '----------------------------------------------------------------------------------------------------------
        'Get rid of the Roles in FactTypes that refer to NORMA UnaryFactType ValueTypes.
        '  NORMA has a ValueType for each UnaryFactType with a corresponding Role attached to the Unary Role of
        '  the FactType. In essense, NORMA has a Binary FactType refering to the Value Type and in Richmond
        '  only the Unary FactType (and singular Role) is required.
        '----------------------------------------------------------------------------------------------------------
        Boston.WriteToStatusBar("Ridding of unnecessary Roles in FactTypes", True, 96)
        Call lrNORMAFileLoader.GetRidOfRolesInFactTypesThatReferToUnaryFactTypeValueTypes(arModel)

        '-------------------------------------------------------
        'Get ModelObjectInstances by Page from the nORMa model
        '-------------------------------------------------------
        Boston.WriteToStatusBar("Loading Page Model Object Instances", True, 97)
        Call lrNORMAFileLoader.LoadPageModelInstances(lrModel, NORMAXMLDOC)

        '-----------------------------------------------------------------------------------------------------
        'Set the Ids of EntityTypes, ValueTypes, FactTypes, RoleConstraints to the same as the Name of
        '  the ModelObject. That is the standard in Boston. In NORMA, each Id is a GUID different from
        '  the name of the ModelObject.
        '-----------------------------------------------------------------------------------------------------
        frmMain.Cursor = Cursors.WaitCursor

        'For Each lrEntityType In lrModel.EntityType
        '    Boston.WriteToStatusBar("Updating Ids for EntityType: " & lrEntityType.Name)
        '    lrEntityType.SetName(lrEntityType.Name)
        'Next

        'For Each lrValueType In lrModel.ValueType
        '    Boston.WriteToStatusBar("Updating Ids for ValueType: " & lrValueType.Name)
        '    lrValueType.SetName(lrValueType.Name)
        'Next

        'For Each lrFactType In lrModel.FactType
        '    Boston.WriteToStatusBar("Updating Ids for FactType: " & lrFactType.Name)
        '    lrFactType.SetName(lrFactType.Name)
        'Next

        'For Each lrRoleConstraint In lrModel.RoleConstraint
        '    Boston.WriteToStatusBar("Updating Ids for RoleConstraint: " & lrFactType.Name)
        '    lrRoleConstraint.SetName(lrRoleConstraint.Name)
        'Next

        'For Each lrPage In lrModel.Page

        '    For Each lrEntityTypeInstance In lrPage.EntityTypeInstance
        '        Boston.WriteToStatusBar("Updating Ids for EntityTypeInstances: " & lrEntityTypeInstance.Name)
        '        lrEntityTypeInstance.SetName(lrEntityTypeInstance.Name)
        '    Next

        '    For Each lrValueTypeInstance In lrPage.ValueTypeInstance
        '        Boston.WriteToStatusBar("Updating Ids for ValueTypeInstances: " & lrValueTypeInstance.Name)
        '        lrValueTypeInstance.SetName(lrValueTypeInstance.Name)
        '    Next

        '    For Each lrFactTypeInstance In lrPage.FactTypeInstance
        '        Boston.WriteToStatusBar("Updating Ids for FactTypeInstances: " & lrFactTypeInstance.Name)
        '        lrFactTypeInstance.SetName(lrFactTypeInstance.Name)
        '    Next

        '    For Each lrRoleConstraintInstance In lrPage.RoleConstraintInstance
        '        lrRoleConstraintInstance.Id = lrRoleConstraintInstance.Name
        '        lrRoleConstraintInstance.Symbol = lrRoleConstraintInstance.Name
        '    Next
        'Next

        lrModel.Loaded = True
        frmMain.Cursor = Cursors.Default

        Boston.WriteToStatusBar("", True, 100)

        frmMain.Cursor = Cursors.Default
        Me.Cursor = Cursors.Default


    End Sub

    Private Sub PastePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemPastePage.Click

        Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")

        Try

            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            '----------------------------------------
            ' Retrieve the data from the clipboard.
            '----------------------------------------
            Dim myRetrievedObject As IDataObject = Clipboard.GetDataObject()

            '----------------------------------------------------
            ' Convert the IDataObject type to MyNewObject type. 
            '----------------------------------------------------
            Dim lrClipboardPage As New FBM.Page 'Clipbrd.ClipboardPage

            lrClipboardPage = CType(myRetrievedObject.GetData(RichmondPage.Name), FBM.Page) ' Clipbrd.ClipboardPage)


            Dim lrPage As New FBM.Page

            If lrClipboardPage Is Nothing Then
                MsgBox("There is no Page in the clipboard to Paste.")
            Else
                lrClipboardPage.Name &= "-Copy"

                lrPage = lrClipboardPage.Clone(prApplication.WorkingModel, True)

                Call Me.AddPageToModel(Me.TreeView.SelectedNode, lrPage)
            End If


            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CopyPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyPageToolStripMenuItem.Click

        Dim liInd As Integer = 0

        If IsSomething(prApplication.WorkingPage) Then

            '------------------------------------------------------------
            'Objects must be serialisable to use the Clipboard.
            '  Use the following to test if the Object is serialisable.
            '------------------------------------------------------------
            Call Boston.IsSerializable(prApplication.WorkingPage)

            '----------------------------
            ' Create a new data format.
            '----------------------------
            Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")

            '------------------------------------------------------------------
            ' Create a new object and store it in a DataObject using myFormat 
            ' as the type of format.                     
            '------------------------------------------------------------------
            Dim dataObj As IDataObject = New DataObject()

            Clipboard.Clear()
            dataObj.SetData(RichmondPage.Name, False, prApplication.WorkingPage)
            Clipboard.SetDataObject(dataObj, False)

        End If
    End Sub

    Private Sub EmptyModel(ByRef arModel As FBM.Model)

        '------------------------------------------------------------------------------------------------------------
        'Remove all the Pages for the Model.
        '  NB Do this processing here because it is easier to remove the TreeNodes from the TreeView on this form.
        '------------------------------------------------------------------------------------------------------------

        Dim lrPage As FBM.Page
        Dim loTreeNode As TreeNode

        Dim liPageCount As Integer = arModel.Page.Count
        For liInd = liPageCount To 1 Step -1
            lrPage = arModel.Page(liInd - 1)

            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            loTreeNode = New TreeNode
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                       lrPage,
                                                       lrPage.Model.ModelId,
                                                       pcenumLanguage.ORMModel,
                                                       Nothing, lrPage.PageId)

            loTreeNode = prPageNodes.Find(AddressOf lr_enterprise_view.Equals).TreeNode

            If IsSomething(loTreeNode) Then
                Me.TreeView.SelectedNode = loTreeNode
                Threading.Thread.Sleep(300)
                Me.TreeView.SelectedNode.Remove()
                Threading.Thread.Sleep(150)
                Me.TreeView.Refresh()

                lrPage.RemoveFromModel()
            Else
                Throw New System.Exception("Cannot find TreeNode for Page")
            End If
        Next

        Call arModel.EmptyModel()

    End Sub

    Private Sub FromORMCMMLFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FromORMCMMLFileToolStripMenuItem.Click

        Dim lsMessage As String = ""

        Me.DialogOpenFile.DefaultExt = "xml"
        Me.DialogOpenFile.Filter = "FBM Files (*.fbm)|*.fbm"
        Me.DialogOpenFile.Filter &= "|XML Files (*.xml)|*.xml"

        Dim lrModel As New FBM.Model
        Dim lrEnterpriseView As New tEnterpriseEnterpriseView

        Try
            lrEnterpriseView = Me.TreeView.SelectedNode.Tag

            lrModel = lrEnterpriseView.Tag

            If Not lrModel.IsEmpty Then

                lsMessage = "The Model is not empty and must be empty before importing a Model."
                lsMessage &= vbCrLf & "Do you wish to empty the Model and continue with the import?"
                lsMessage &= vbCrLf & vbCrLf & "All Model Objects will be removed from the Model. This operation cannot be Undone."

                If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Call Me.EmptyModel(lrModel)
                Else
                    Exit Sub
                End If

            End If

            If Me.DialogOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then


                'Deserialize text file to a new object.
                Dim objStreamReader As New StreamReader(Me.DialogOpenFile.FileName)


                '=====================================================================================================
                'Find the version of the XML's XSD                        
                Dim xml As XDocument
                Dim lsXSDVersionNr As String = ""

                xml = XDocument.Load(Me.DialogOpenFile.FileName)

                lsXSDVersionNr = xml.<Model>.@XSDVersionNr
                '=====================================================================================================
                Dim lrSerializer As XmlSerializer = Nothing
                Select Case lsXSDVersionNr
                    Case Is = "0.81"
                        lrSerializer = New XmlSerializer(GetType(XMLModelv081.Model))
                        Dim lrXMLModel As New XMLModelv081.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                    Case Is = "1"
                        lrSerializer = New XmlSerializer(GetType(XMLModel1.Model))
                        Dim lrXMLModel As New XMLModel1.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                    Case Is = "1.1"
                        lrSerializer = New XmlSerializer(GetType(XMLModel.Model))
                        Dim lrXMLModel As New XMLModel.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                    Case Is = "1.2"
                        lrSerializer = New XmlSerializer(GetType(XMLModel12.Model))
                        Dim lrXMLModel As New XMLModel12.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                    Case Is = "1.3"
                        lrSerializer = New XmlSerializer(GetType(XMLModel13.Model))
                        Dim lrXMLModel As New XMLModel13.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                    Case Is = "1.4"
                        lrSerializer = New XmlSerializer(GetType(XMLModel14.Model))
                        Dim lrXMLModel As New XMLModel14.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                    Case Is = "1.5"
                        lrSerializer = New XmlSerializer(GetType(XMLModel15.Model))
                        Dim lrXMLModel As New XMLModel15.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                    Case Is = "1.7"
                        lrSerializer = New XmlSerializer(GetType(XMLModel.Model))
                        Dim lrXMLModel As New XMLModel.Model
                        lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                        objStreamReader.Close()
                        lrModel = lrXMLModel.MapToFBMModel
                End Select

                '================================================================================================================
                'RDS
                If lrModel.HasCoreModel Then
                    Using lrWaitCursor As New WaitCursor
                        Call lrModel.PopulateAllCoreStructuresFromCoreMDAElements()
                    End Using
                Else
                    '==================================================
                    'RDS - Create a CMML Page and then dispose of it.
                    Dim lrPage As FBM.Page
                    Dim lrCorePage As FBM.Page

                    '    (prApplication.CMML.Core, _
                    '    pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString, _
                    '    pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString, _
                    '    pcenumLanguage.ORMModel)

                    lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                    If lrCorePage Is Nothing Then
                        Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                    End If

                    '----------------------------------------------------
                    'Create the Page for the EntityRelationshipDiagram.
                    '----------------------------------------------------
                    lrPage = lrCorePage.Clone(lrModel, False, True, False)
                    '==================================================

                    Using lrWaitCursor As New WaitCursor
                        Call lrModel.createEntityRelationshipArtifacts()
                    End Using
                End If

                '-----------------------------------------
                'Update the TreeView
                '-----------------------------------------
                lrEnterpriseView.Tag = lrModel
                Call Me.AddPagesForModel(lrModel, Me.TreeView.SelectedNode)

                Me.TreeView.SelectedNode.Expand()

                Using lrWaitCursor As New WaitCursor
                    Call lrModel.Save(True)
                End Using

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ExportTestingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportTestingToolStripMenuItem.Click

        Dim lsFolderLocation As String = ""
        Dim lsFileName As String = ""
        Dim loStreamWriter As StreamWriter ' Create file by FileStream class
        Dim loXMLSerialiser As XmlSerializer ' Create binary object
        Dim lrModel As FBM.Model
        Dim lrExportModel As New XMLModel.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            lrExportModel.ORMModel.ModelId = lrModel.ModelId
            lrExportModel.ORMModel.Name = lrModel.Name

            If My.Settings.ExportFBMExcludeMDAModelElements Then
                If MsgBox("Important: Your configuration settings will only allow the export of Object-Role Models. Are you happy to proceed?", MsgBoxStyle.YesNoCancel) <> MsgBoxResult.Yes Then
                    Exit Sub
                End If
            End If

            If Not lrExportModel.MapFromFBMModel(lrModel, My.Settings.ExportFBMExcludeMDAModelElements) Then
                MsgBox("Fix the model errors, then try again.")
                Exit Sub
            End If

            Dim lsFileLocationName As String = ""
            If Boston.IsSerializable(lrExportModel) Then

                If My.Settings.UseClientServer And My.Settings.UseVirtualUI Then
                    lsFolderLocation = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData
                    lsFileName = prApplication.User.Id & "-" & lrModel.Name & ".fbm"
                    lsFileLocationName = lsFolderLocation & "\" & lsFileName
                Else
                    Dim lrSaveFileDialog As New SaveFileDialog()

                    lsFileName = lrModel.Name & ".fbm"
                    lsFileLocationName = lsFileName

                    lrSaveFileDialog.Filter = "Fact-Based Model file (*.fbm)|*.fbm"
                    lrSaveFileDialog.FilterIndex = 0
                    lrSaveFileDialog.RestoreDirectory = True
                    lrSaveFileDialog.FileName = lsFileLocationName

                    If lrSaveFileDialog.ShowDialog() = DialogResult.OK Then
                        lsFileLocationName = lrSaveFileDialog.FileName
                    Else
                        Exit Sub
                    End If

                    'If DialogFolderBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '    lsFolderLocation = DialogFolderBrowser.SelectedPath
                    'Else
                    '    Exit Sub
                    'End If
                End If

                loStreamWriter = New StreamWriter(lsFileLocationName) 'lsFolderLocation & "\" & lsFileName)

                'loXMLSerialiser = New XmlSerializer(GetType(FBM.tORMModel))
                loXMLSerialiser = New XmlSerializer(GetType(XMLModel.Model))

                'Serialize object to file
                loXMLSerialiser.Serialize(loStreamWriter, lrExportModel)
                loStreamWriter.Close()

                If My.Settings.UseClientServer And My.Settings.UseVirtualUI Then
                    prThinfinity.DownloadFile(lsFileLocationName)
                End If

                Dim lsMessage As String = ""
                lsMessage = "Your file is ready for viewing."
                If Not My.Settings.UseClientServer Then
                    lsMessage &= vbCrLf & vbCrLf
                    lsMessage &= lsFileLocationName
                End If

                MsgBox(lsMessage)

            End If 'IsSerialisable

        Catch ex As Exception
            Dim lsMessage As String = ""
            lsMessage = "Error: frnToolboxEnterpriseTree.ExportToORMCMMLToolStripMenuItem: " & vbCrLf & vbCrLf & ex.Message
            Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        End Try

    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click

        Call Me.ImportFBMXMLFile()

    End Sub

    Public Sub ImportFBMXMLFile()

        Dim lsMessage As String
        Me.DialogOpenFile.DefaultExt = "xml"
        Me.DialogOpenFile.Filter = "FBM Files (*.fbm)|*.fbm"
        Me.DialogOpenFile.Filter &= "|XML Files (*.xml)|*.xml"

        If Me.DialogOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            Try
                With New WaitCursor
                    '=====================================================================================================
                    'Find the version of the XML's XSD                        
                    Call Me.loadFBMXMLFile2(Me.DialogOpenFile.FileName)
                End With

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                frmMain.Cursor = Cursors.Default

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End If

    End Sub

    Public Sub loadFBMXMLFile2(ByVal asFileName As String)

        Try
            Dim xml As XDocument = Nothing
            Dim lsXSDVersionNr As String = ""
            Dim lrModel As New FBM.Model
            Dim lsMessage As String

            If asFileName = "" Then
                Exit Sub
            End If

            'Deserialize text file to a new object.
            Dim objStreamReader As New StreamReader(asFileName)

            xml = XDocument.Load(asFileName)

            Boston.WriteToStatusBar("Loading model.", True)

            lsXSDVersionNr = xml.<Model>.@XSDVersionNr
            '=====================================================================================================
            Dim lrSerializer As XmlSerializer = Nothing
            Select Case lsXSDVersionNr
                Case Is = "0.81"
                    lrSerializer = New XmlSerializer(GetType(XMLModelv081.Model))
                    Dim lrXMLModel As New XMLModelv081.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1"
                    lrSerializer = New XmlSerializer(GetType(XMLModel1.Model))
                    Dim lrXMLModel As New XMLModel1.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.1"
                    lrSerializer = New XmlSerializer(GetType(XMLModel11.Model))
                    Dim lrXMLModel As New XMLModel11.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.2"
                    lrSerializer = New XmlSerializer(GetType(XMLModel12.Model))
                    Dim lrXMLModel As New XMLModel12.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.3"
                    lrSerializer = New XmlSerializer(GetType(XMLModel13.Model))
                    Dim lrXMLModel As New XMLModel13.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()

                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.4"
                    lrSerializer = New XmlSerializer(GetType(XMLModel14.Model))
                    Dim lrXMLModel As New XMLModel14.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.5"
                    lrSerializer = New XmlSerializer(GetType(XMLModel15.Model))
                    Dim lrXMLModel As New XMLModel15.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.6"
                    lrSerializer = New XmlSerializer(GetType(XMLModel16.Model))
                    Dim lrXMLModel As New XMLModel16.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.7"
                    lrSerializer = New XmlSerializer(GetType(XMLModel.Model))
                    Dim lrXMLModel As New XMLModel.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
            End Select

            If TableModel.ExistsModelById(lrModel.ModelId) Then
                lsMessage = "A Model with the Id: " & lrModel.ModelId
                lsMessage &= vbCrLf & "already exists in the database."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "The Model that you are loading will be given a new Id. All Pages within the Model will also be given a new Id."
                lsMessage &= vbCrLf & "NB The name of the Model ('" & lrModel.Name & "') will stay the same if there is no other Model in the database with the same name."
                lrModel.ModelId = System.Guid.NewGuid.ToString
                '---------------------------------------------------------------------------------------------
                'All of the Page.Ids must be updated as well, as each PageId is unique in the database.
                '  i.e. If the Model is not unique, there's a very good chance that neither are the PageIds.
                '---------------------------------------------------------------------------------------------
                Dim lrPage As FBM.Page
                For Each lrPage In lrModel.Page
                    lrPage.PageId = System.Guid.NewGuid.ToString
                Next

                lrModel.MakeDirty(False, True)

                'MsgBox(lsMessage)
            End If

            If TableModel.ExistsModelByName(lrModel.Name) Then
                lsMessage = "A Model with the Name: " & lrModel.Name
                lsMessage &= vbCrLf & "already exists in the database."
                lsMessage &= vbCrLf & vbCrLf
                lrModel.Name = lrModel.CreateUniqueModelName(lrModel.Name, 0)
                lsMessage &= "The Model that you are loading will be given the new Name: " & lrModel.Name
                'MsgBox(lsMessage)
            End If

            'Project 
            If prApplication.WorkingProject Is Nothing Then prApplication.WorkingProject = New ClientServer.Project("MyPersonalModels", "MyPersonalModels")
            lrModel.ProjectId = prApplication.WorkingProject.Id

            'Namespace
            If prApplication.WorkingProject.Id = "MyPersonalModels" Then
                lrModel.Namespace = Nothing
            Else
                lrModel.Namespace = Me.ComboBoxNamespace.SelectedItem.Tag
            End If

            If My.Settings.UseClientServer And (prApplication.User IsNot Nothing) Then
                lrModel.CreatedByUserId = prApplication.User.Id
            End If

            '-----------------------------------------
            'Update the TreeView
            '-----------------------------------------
            Dim lrNewTreeNode = Me.AddModelToModelExplorer(lrModel, False)

            lrNewTreeNode.Expand()
            Me.TreeView.Nodes(0).Nodes(Me.TreeView.Nodes(0).Nodes.Count - 1).EnsureVisible()

            'Boston.WriteToStatusBar("Saving model.", True)
            Dim lfrmFlashCard As New frmFlashCard
            '20220129-VM-Commented out.
            'lfrmFlashCard.ziIntervalMilliseconds = 1500
            'lfrmFlashCard.zsText = "Saving model."
            'lfrmFlashCard.Show(frmMain)
            'Me.Focus()

            'Call lrModel.Save(True, False)

            '================================================================================================================
            'RDS
            If (lrModel.ModelId <> "Core") And lrModel.HasCoreModel Then
                Call lrModel.performCoreManagement(False)
                Call lrModel.PopulateAllCoreStructuresFromCoreMDAElements()
                lrModel.RDSCreated = True
            ElseIf (lrModel.ModelId <> "Core") Then
                '==================================================
                'RDS - Create a CMML Page and then dispose of it.
                Dim lrPage As FBM.Page '(lrModel)
                Dim lrCorePage As FBM.Page

                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Clone the Page Model Elements for the EntityRelationshipDiagram into the metamodel

                'StateTransitionDiagrams
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Clone the Page Model Elements for the StateTransitionDiagram into the metamodel

                'Derivations
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreDerivations.ToString) 'AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreDerivations.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(lrModel, False, True, False) 'Clone the Page Model Elements for the CoreDerivations into the metamodel

                'UseCaseDiagrams
                lrCorePage = prApplication.CMML.Core.Page.Find(Function(x) x.Name = pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(lrModel, False, True, False)
                '==================================================

                Call lrModel.createEntityRelationshipArtifacts()
                Call lrModel.PopulateAllCoreStructuresFromCoreMDAElements()
                lrModel.RDSCreated = True
            End If

            frmMain.Cursor = Cursors.Default

            'Baloon Tooltip
            lsMessage = "Loaded"
            Me.zrToolTip.IsBalloon = True
            Me.zrToolTip.ToolTipIcon = ToolTipIcon.None
            Me.zrToolTip.ShowAlways = True
            Me.zrToolTip.Active = True 'turns On the tooltip
            Me.zrToolTip.AutomaticDelay = 0 'some auto value that will fill others With Default values
            Me.zrToolTip.AutoPopDelay = 3000 'how Long it will stay before vanishing
            Me.zrToolTip.InitialDelay = 0 'how Long you need To keep your mouse cursor still before it reacts                
            Me.zrToolTip.Show(lsMessage, Me, lrNewTreeNode.Bounds.X, lrNewTreeNode.Bounds.Y + 20 + lrNewTreeNode.Bounds.Height, 4000)


            '----------------------------------------------------------------------------------------------------------------
            'Saving the Model
            Dim lrCustomMessageBox As New frmCustomMessageBox

            lsMessage = "Your Model has been successfully loaded into Boston." & vbCrLf & vbCrLf
            lsMessage &= "Save the model now? (Recommended)"

            lrCustomMessageBox.Message = lsMessage
            lrCustomMessageBox.ButtonText.Add("No")
            lrCustomMessageBox.ButtonText.Add("Save to database")
            lrCustomMessageBox.ButtonText.Add("Store as XML")

            lfrmFlashCard = New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 3500
            lfrmFlashCard.zsText = "Saving model."

            Select Case lrCustomMessageBox.ShowDialog
                Case Is = "Store as XML"
                    lrModel.StoreAsXML = True
                    Boston.WriteToStatusBar("Saving Model: " & lrModel.Name)
                    Call lrModel.Save(True, False)
                    Boston.WriteToStatusBar("Model Saved")
                Case Is = "Save to database"
                    With New WaitCursor
                        Boston.WriteToStatusBar("Saving Model: " & lrModel.Name)
                        lfrmFlashCard.Show(Me)
                        lrModel.StoreAsXML = False
                        Call lrModel.Save(True, True)
                        Boston.WriteToStatusBar("Model Saved")
                    End With
            End Select

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            frmMain.Cursor = Cursors.Default

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    '20200725-VM-Remove the below if all okay. Replaced with LoadFBMXMLFile2 in this document.
    'Public Sub LoadFBMXMLFile(ByVal asFilePathName As String)

    '    Try
    '        If asFilePathName = "" Then
    '            Exit Sub
    '        End If

    '        prApplication.ThrowErrorMessage("About to deserialise the model from .fbm file: " & asFilePathName, pcenumErrorType.Information)


    '        'Deserialize text file to a new object.
    '        Dim objStreamReader As New StreamReader(asFilePathName)
    '        Dim p2 As New XMLModel.Model
    '        Dim x As New XmlSerializer(GetType(XMLModel.Model))
    '        p2 = x.Deserialize(objStreamReader)
    '        objStreamReader.Close()

    '        prApplication.ThrowErrorMessage("Successfully deserialised the model from .fbm file: " & asFilePathName, pcenumErrorType.Information)

    '        Dim lrModel As New FBM.Model

    '        lrModel = p2.MapToFBMModel

    '        '====================================================================================
    '        Dim lsMessage As String = ""
    '        If TableModel.ExistsModelById(lrModel.ModelId) Then
    '            lsMessage = "A Model with the Id: " & lrModel.ModelId
    '            lsMessage &= vbCrLf & "already exists in the database."
    '            lsMessage &= vbCrLf & vbCrLf
    '            lsMessage &= "The Model that you are loading will be given a new Id. All Pages within the Model will also be given a new Id."
    '            lsMessage &= vbCrLf & "NB The name of the Model ('" & lrModel.Name & "') will stay the same if there is no other Model in the database with the same name."
    '            lrModel.ModelId = System.Guid.NewGuid.ToString
    '            '---------------------------------------------------------------------------------------------
    '            'All of the Page.Ids must be updated as well, as each PageId is unique in the database.
    '            '  i.e. If the Model is not unique, there's a very good chance that neither are the PageIds.
    '            '---------------------------------------------------------------------------------------------
    '            Dim lrPage As FBM.Page
    '            For Each lrPage In lrModel.Page
    '                lrPage.PageId = System.Guid.NewGuid.ToString
    '            Next

    '            lrModel.MakeDirty(False, True)

    '            MsgBox(lsMessage)
    '        End If

    '        If TableModel.ExistsModelByName(lrModel.Name) Then
    '            lsMessage = "A Model with the Name: " & lrModel.Name
    '            lsMessage &= vbCrLf & "already exists in the database."
    '            lsMessage &= vbCrLf & vbCrLf
    '            lrModel.Name = lrModel.CreateUniqueModelName(lrModel.Name, 0)
    '            lsMessage &= "The Model that you are loading will be given the new Name: " & lrModel.Name
    '            MsgBox(lsMessage)
    '        End If
    '        '====================================================================================

    '        prApplication.ThrowErrorMessage("Successfully mapped the model from .fbm file: " & asFilePathName, pcenumErrorType.Information)

    '        '-----------------------------------------
    '        'Update the TreeView
    '        '-----------------------------------------
    '        Call Me.AddModelToModelExplorer(lrModel, False)

    '        Directory.SetCurrentDirectory(Boston.MyPath)

    '    Catch ex As Exception
    '        Dim lsMessage As String
    '        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

    '        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
    '        lsMessage &= vbCrLf & vbCrLf & ex.Message
    '        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
    '    End Try

    'End Sub


    Private Sub ContextMenuStrip_Page_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Page.Opening

        Try
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            Dim lrPage As FBM.Page

            Me.ContextMenuStrip_Page.ImageScalingSize = New Drawing.Size(16, 16)

            If Nothing Is Me.TreeView.SelectedNode Then
                Me.ToolStripMenuItemEditPageAsORMDiagram.Visible = False
            End If

            '----------------------------------------
            'Create a temporary object for the Page
            '----------------------------------------
            lrEnterpriseView = Me.TreeView.SelectedNode.Tag
            lrPage = lrEnterpriseView.Tag


            If My.Settings.SuperuserMode And Not lrPage.Language = pcenumLanguage.ORMModel Then
                Me.ToolStripMenuItemEditPageAsORMDiagram.Visible = True
            Else
                Me.ToolStripMenuItemEditPageAsORMDiagram.Visible = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemEditPageAsORMDiagram_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemEditPageAsORMDiagram.Click

        Dim lrEnterpriseView As tEnterpriseEnterpriseView
        Dim lrPage As FBM.Page

        If Nothing Is Me.TreeView.SelectedNode Then
            Exit Sub
        End If

        lrEnterpriseView = Me.TreeView.SelectedNode.Tag
        lrPage = lrEnterpriseView.Tag

        Call frmMain.loadORMModelPage(lrPage, Me.TreeView.SelectedNode)

    End Sub

    Public Sub LoadSelectedPage(Optional ByVal asSelectModelElementId As String = Nothing)

        Try

            frmMain.Cursor = Cursors.WaitCursor
            Me.Cursor = Cursors.WaitCursor
            Me.Invalidate()

            If Me.TreeView.SelectedNode Is Nothing Then
                Throw New Exception("Must have a TreeNode selected to call this method")
            End If

            Dim lrPage As FBM.Page
            Dim lrEnterpriseView As New tEnterpriseEnterpriseView

            '----------------------------------------
            'Create a temporary object for the Page
            '----------------------------------------
            'lrPage = New FBM.Page(prApplication.workingmodel, prApplication.workingpage.PageId, prApplication.workingpage.Name)
            lrEnterpriseView = Me.TreeView.SelectedNode.Tag
            lrPage = lrEnterpriseView.Tag 'prApplication.WorkingPage 

            If Not lrPage.Model.StoreAsXML And lrPage.Loaded = False And Not lrPage.Loading Then
                With New WaitCursor
                    lrPage.Load(False)
                End With
            End If

            If lrPage.Loading Or ((lrPage.Language <> pcenumLanguage.ORMModel) And (lrPage.Model.RDSLoading Or lrPage.Model.STMLoading)) Then
                Boston.WriteToStatusBar("Waiting for background loading of the Page", True)
                frmMain.Cursor = Cursors.WaitCursor
                frmMain.Refresh()
            End If

            Dim stopwatch As New Stopwatch()
            stopwatch.Start()
            While lrPage.Loading Or ((lrPage.Language <> pcenumLanguage.ORMModel) And (lrPage.Model.RDSLoading Or lrPage.Model.STMLoading))
                '---------------------------------------------
                'Wait for threads to finish loading the Page
                '---------------------------------------------
                If stopwatch.ElapsedMilliseconds > 20000 Then Exit While
            End While
            stopwatch.Stop()

            '-------------------------------------------------------
            'Check if the page has already been opened for editing
            '-------------------------------------------------------
            If lrPage.FormLoaded And lrPage.Form IsNot Nothing Then
                '-------------------------------------------------------------------
                'The Page has already been loaded for Editing
                '  Set the ZOrder of the Form on which the Page is loaded to OnTop
                '-------------------------------------------------------------------

                lrPage.Form.BringToFront(asSelectModelElementId)
                prApplication.WorkingPage = lrPage

            Else
                '--------------------------------------------------------------------------------------------------
                'Add the Page to the Model.
                '  NB The Model is loaded when the User clicks on the Model in the TreeView, so is already loaded.
                '--------------------------------------------------------------------------------------------------  
                'lr_enterprise_view = New tEnterpriseView(pcenumMenuType.Page_use_case_diagram, lrPage, lr_model.enterpriseid, lr_model.SubjectAreaId, lr_model.projectId, lr_model.solution_id, lr_model.ModelId, pcenumLanguage.UseCaseDiagram, Nothing, lrPage.PageId)
                'lo_menu_option.Tag = frm_main.zfrm_enterprise_tree_viewer.prPageNodes.Find(AddressOf lr_enterprise_view.Equals)

                prApplication.ThrowErrorMessage("[Edit Page] Clicked. Load Page request being sent to frmMain.", pcenumErrorType.Information)

                Select Case lrPage.Language
                    Case Is = pcenumLanguage.ORMModel
                        Call frmMain.loadORMModelPage(lrPage, Me.TreeView.SelectedNode)
                    Case Is = pcenumLanguage.EntityRelationshipDiagram
                        Call frmMain.loadERDiagramView(lrPage, Me.TreeView.SelectedNode)
                    Case Is = pcenumLanguage.PropertyGraphSchema
                        Call frmMain.loadPGSDiagramView(lrPage, Me.TreeView.SelectedNode, asSelectModelElementId)
                    Case Is = pcenumLanguage.StateTransitionDiagram
                        Call frmMain.load_StateTransitionDiagram_view(lrPage, Me.TreeView.SelectedNode, True)
                    Case Is = pcenumLanguage.UMLUseCaseDiagram
                        Call frmMain.loadUMLUseCaseDiagramView(lrPage, Me.TreeView.SelectedNode, True)
                    Case Is = pcenumLanguage.BPMNChoreographDiagram
                        Call frmMain.loadBPMNChoreographyDiagramView(lrPage, Me.TreeView.SelectedNode, True)
                    Case Is = pcenumLanguage.BPMNCollaborationDiagram
                        Call frmMain.loadBPMNCollaborationDiagramView(lrPage, Me.TreeView.SelectedNode, True)
                    Case Is = pcenumLanguage.BPMNConversationDiagram
                        Call frmMain.loadBPMNConversationDiagramView(lrPage, Me.TreeView.SelectedNode, True)
                    Case Is = pcenumLanguage.BPMNProcessDigram
                        Call frmMain.loadBPMNProcessDiagramView(lrPage, Me.TreeView.SelectedNode, True)
                End Select

                'Select Case Me.TreeView.SelectedNode.Tag.MenuType
                '    Case Is = pcenumMenuType.pageORMModel
                '        Call frmMain.loadORMModelPage(lrPage, Me.TreeView.SelectedNode)
                '    Case Is = pcenumMenuType.pageERD
                '        If lrPage.Language = pcenumLanguage.EntityRelationshipDiagram Then
                '            Call frmMain.load_ER_diagram_view(lrPage, Me.TreeView.SelectedNode)
                '        Else
                '            Call frmMain.loadORMModelPage(lrPage, Me.TreeView.SelectedNode)
                '        End If
                '    Case Is = pcenumMenuType.pagePGSDiagram
                '        If lrPage.Language = pcenumLanguage.PropertyGraphSchema Then
                '            Call frmMain.load_PGS_diagram_view(lrPage, Me.TreeView.SelectedNode, asSelectModelElementId)
                '        Else
                '            Call frmMain.loadORMModelPage(lrPage, Me.TreeView.SelectedNode)
                '        End If
                'End Select

            End If

            If lrEnterpriseView.FocusModelElement IsNot Nothing Then

                Try
                    Dim lsMessage As String = ""

                    lsMessage = lrEnterpriseView.FocusModelElement.Id

                    If lrEnterpriseView.FocusModelElement.ConceptType = pcenumConceptType.ValueType Then
                        Dim larEntityTypeInstance = From EntityTypeInstance In lrPage.EntityTypeInstance
                                                    Where EntityTypeInstance.ReferenceModeValueType IsNot Nothing
                                                    Select EntityTypeInstance

                        For Each lrEntityTypeInstance In larEntityTypeInstance
                            If lrEntityTypeInstance.ReferenceModeValueType.Id = lrEnterpriseView.FocusModelElement.Id Then
                                lsMessage = lrEnterpriseView.FocusModelElement.Id & " provides the Reference Mode Value Type for Entity Type, '" & lrEntityTypeInstance.Id & "'"
                                lrEnterpriseView.FocusModelElement = lrEntityTypeInstance
                            End If
                        Next

                    End If

                    Dim lrShapeNode As MindFusion.Diagramming.ShapeNode

                    Select Case lrEnterpriseView.FocusModelElement.ConceptType
                        Case Is = pcenumConceptType.RoleConstraint
                            Dim lrRoleConstraint As FBM.RoleConstraint = lrEnterpriseView.FocusModelElement
                            Select Case lrRoleConstraint.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                                    lrRoleConstraintInstance = lrPage.GetAllPageObjects(True).Find(Function(x) x.Id = lrEnterpriseView.FocusModelElement.Id)
                                    lrShapeNode = lrRoleConstraintInstance.RoleConstraintRole(0).Shape
                                Case Else
                                    lrShapeNode = lrPage.GetAllPageObjects(True).Find(Function(x) x.Id = lrEnterpriseView.FocusModelElement.Id).Shape
                            End Select
                        Case Else
                            lrShapeNode = lrPage.GetAllPageObjects().Find(Function(x) x.Id = lrEnterpriseView.FocusModelElement.Id).Shape
                    End Select

                    If lrShapeNode IsNot Nothing Then

                        Dim lrForm As frmDiagramORM = lrPage.Form

                        Dim liX, liY As Integer
                        liX = lrShapeNode.Bounds.X
                        liY = lrShapeNode.Bounds.Y
                        Dim lrPoint As New Point(liX + 5, liY + 30)
                        Dim lrFinalPoint As PointF = lrPage.DiagramView.DocToClient(lrPoint)
                        lrFinalPoint.X += lrForm.Left
                        lrFinalPoint.Y += lrForm.Top

                        Dim lrToolTip As New BalloonTip
                        'lrToolTip.IsBalloon = True
                        'lrToolTip.ToolTipIcon = ToolTipIcon.None
                        'lrToolTip.BackColor = Color.Yellow
                        'lrToolTip.ForeColor = Color.White
                        'lrToolTip.OwnerDraw = True
                        lrToolTip.Show("", lsMessage, lrPage.Form, New Point(lrFinalPoint.X, lrFinalPoint.Y), 0, 4000)

                    End If
                Catch ex As Exception

                End Try
                lrEnterpriseView.FocusModelElement = Nothing
            End If

            Boston.WriteToStatusBar("Editing Page: '" & lrPage.Name & "'", True)

            frmMain.ToolStripButtonPrint.Enabled = True

            frmMain.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TreeView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView.DoubleClick

        Dim lrEnterpriseView As New tEnterpriseEnterpriseView

        With New WaitCursor
            If IsSomething(Me.TreeView.SelectedNode) Then

                lrEnterpriseView = Me.TreeView.SelectedNode.Tag

                '--------------------------------------------------------------
                'The Zero Level Node ("Boston") has no Tag.
                If lrEnterpriseView.Tag Is Nothing Then Exit Sub

                If lrEnterpriseView.Tag.GetType Is GetType(FBM.Page) Then

                    '==================================================================================
                    'Only open the Page if the User has the correct Permissions for the Poject
                    If My.Settings.UseClientServer Then
                        If prApplication.User.ProjectPermission.FindAll(Function(x) x.Permission = pcenumPermission.NoRights).Count > 0 Then
                            '-------------------------------------------------------------------
                            'User has no Permission to Create/Read/Alter the Page, so don't load it
                            Dim lfrmFlashCard As New frmFlashCard
                            lfrmFlashCard.ziIntervalMilliseconds = 3500
                            lfrmFlashCard.zsText = "You do not have Permission to Create, Read or Alter Models within this Project."
                            Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(Me)

                            Exit Sub
                        End If
                    End If

                    If IsSomething(frmMain.zfrmStartup) Then
                        frmMain.zfrmStartup.Close()
                        frmMain.zfrmStartup = Nothing
                    End If

                    frmMain.Cursor = Cursors.WaitCursor
                    Call Me.LoadSelectedPage()
                End If

            End If
        End With

    End Sub

    Public Sub UpdateProgress(ByVal asngProgress As Single)

        Me.CircularProgressBar.Value = CInt(asngProgress)
        Me.CircularProgressBar.Text = CInt(asngProgress).ToString & "%"
        Me.CircularProgressBar.Invalidate()
        Me.Invalidate(True)
    End Sub

    Public Sub ResetProgress()
        Me.CircularProgressBar.BringToFront()
        Me.CircularProgressBar.Value = 0
        Me.CircularProgressBar.Text = "0%"
        Me.CircularProgressBar.Invalidate()
        Me.Invalidate(True)
        Me.CircularProgressBar.SendToBack()
    End Sub

    Private Sub frmToolboxEnterpriseExplorer_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged

        'Me.CircularProgressBar.Location = New Point(CInt((2 / 4) * Me.Width), Me.CircularProgressBar.Location.Y)
        Me.TreeView.Left = 3

    End Sub

    Private Sub BackgroundWorkerModelLoader_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerModelLoader.ProgressChanged

        'CodeSafe: Don't let a percentage get greater than 100
        Dim liPercentage As Integer = Viev.Lesser(100, e.ProgressPercentage)

        Me.Invoke(New Action(Sub()
                                 Me.CircularProgressBar.Value = 0
                                 Me.CircularProgressBar.Value = liPercentage
                                 Me.CircularProgressBar.Text = liPercentage & "%"
                                 Me.CircularProgressBar.Invalidate()
                                 Me.TreeView.Refresh()
                                 Me.CircularProgressBar.Refresh()
                                 Me.Refresh()
                             End Sub
            ))

    End Sub


    Private Sub BackgroundWorkerModelLoader_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerModelLoader.RunWorkerCompleted

        Call Me.CircularProgressBar.SendToBack()

    End Sub

    Private Sub ModelConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemModelConfiguration.Click

        Dim lrModel As FBM.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = New FBM.Model
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            Call frmMain.LoadCRUDModel(lrModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub AddPGSPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddPGSPageToolStripMenuItem.Click

        Try
            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            '==============================================================
            'Get the Core Metamodel.Page for an EntityRelationshipDiagram
            ' NB Is the same metamodel as used for PropertyGraphSchemas
            '==============================================================
            '-------------------------------------------
            'Get the EntityRelationshipModel Core Page
            '-------------------------------------------
            Using lrWaitCursor As New WaitCursor
                Dim lrPage As FBM.Page

                Boston.WriteToStatusBar("Loading the MetaModel for Property Graph Schemas.")

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString,
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CorePropertyGraphSchema.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the EntityRelationshipDiagram.
                '----------------------------------------------------
                Boston.WriteToStatusBar("Creating the Page.")
                lrPage = lrCorePage.Clone(prApplication.WorkingModel)
                lrPage.Name = prApplication.WorkingModel.CreateUniquePageName("NewPropertyGraphSchema", 0)
                lrPage.Language = pcenumLanguage.PropertyGraphSchema

                Call Me.AddPageToModel(Me.TreeView.SelectedNode, lrPage, False, True)

                Call lrPage.Save()

            End Using

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AddERDPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddERDPageToolStripMenuItem.Click

        Try
            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            '==============================================================
            'Get the Core Metamodel.Page for an EntityRelationshipDiagram
            '==============================================================
            '-------------------------------------------
            'Get the EntityRelationshipModel Core Page
            '-------------------------------------------
            Dim lrPage As FBM.Page

            Using lrWaitCursor As New WaitCursor
                Boston.WriteToStatusBar("Loading the MetaModel for Entity Relationship Diagrams")

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString,
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the EntityRelationshipDiagram.
                '----------------------------------------------------
                Boston.WriteToStatusBar("Creating the Page.")
                lrPage = lrCorePage.Clone(prApplication.WorkingModel)
                lrPage.Name = prApplication.WorkingModel.CreateUniquePageName("NewEntityRelationshipDiagram", 0)
                lrPage.Language = pcenumLanguage.EntityRelationshipDiagram

                Call Me.AddPageToModel(Me.TreeView.SelectedNode, lrPage, False,, True)

                Call lrPage.Save()

            End Using 'WaitCursor

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TreeView_BeforeLabelEdit(sender As Object, e As NodeLabelEditEventArgs) Handles TreeView.BeforeLabelEdit

        If e.Node.Level = 0 Then
            e.CancelEdit = True
        Else
            Me.TreeView.LabelEdit = True
        End If

    End Sub

    Private Sub ViewGlossaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewGlossaryToolStripMenuItem.Click

        With New WaitCursor

            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag

            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
            End If

            While (prApplication.WorkingModel.Loading And Not prApplication.WorkingModel.Loaded) Or prApplication.WorkingModel.Page.FindAll(Function(x) x.Loading).Count > 0
                Boston.WriteToStatusBar("Still loading the Model's Pages")
            End While

            Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)

            Call frmMain.LoadGlossaryForm()
        End With

    End Sub

    Public Sub LoadProjects()

        Try

            With New WaitCursor

                Dim lrProject As ClientServer.Project
                Dim larProject As New List(Of ClientServer.Project)

                Call tableClientServerProject.GetProjects(larProject, prApplication.User)

                Dim lrComboboxItem As tComboboxItem

                Me.zbLoadingProjects = True 'So that changing the Index item doesn't trigger loading Namespaces for each Project loaded

                lrProject = New ClientServer.Project("MyPersonalModels", "My Personal Models")
                lrComboboxItem = New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
                Me.ComboBoxProject.Items.Add(lrComboboxItem)

                For Each lrProject In larProject
                    lrComboboxItem = New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
                    Me.ComboBoxProject.Items.Add(lrComboboxItem)
                Next
                Me.zbLoadingProjects = False


                Me.ComboBoxProject.SelectedIndex = Me.ComboBoxProject.FindString("My Personal Models")
            End With

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxProject_Enter(sender As Object, e As EventArgs) Handles ComboBoxProject.Enter

        Me.ziSelectedProjectIndex = Me.ComboBoxProject.SelectedIndex

    End Sub


    ''' <summary>
    ''' Removes all the Models from the ModelExplorer (and from within the list within prApplication.Model).
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub clearModels()

    End Sub

    Private Sub loadNamespacesForProject(ByRef arProject As ClientServer.Project)

        Try
            Dim lrNamespace As ClientServer.Namespace
            Dim larNamespace As New List(Of ClientServer.Namespace)

            larNamespace = tableClientServerNamespace.getNamespacesForProject(arProject)

            Dim lrComboboxItem As tComboboxItem

            Me.ComboBoxNamespace.Items.Clear()

            If arProject.Id = "MyPersonalModels" Then
                lrNamespace = New ClientServer.Namespace(0, "N/A", arProject)
                lrComboboxItem = New tComboboxItem(lrNamespace.Id, lrNamespace.Name, lrNamespace)
                Me.ComboBoxNamespace.Items.Add(lrComboboxItem)
            Else
                For Each lrNamespace In larNamespace
                    lrComboboxItem = New tComboboxItem(lrNamespace.Id, lrNamespace.Name, lrNamespace)
                    Me.ComboBoxNamespace.Items.Add(lrComboboxItem)
                Next
            End If

            If Me.ComboBoxNamespace.Items.Count > 0 Then
                Me.ComboBoxNamespace.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxProject_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBoxProject.SelectionChangeCommitted

        If Me.ComboBoxProject.SelectedIndex < 0 Then Exit Sub

        Dim lrProject As ClientServer.Project
        lrProject = Me.ComboBoxProject.SelectedItem.Tag
        Call Me.loadNamespacesForProject(lrProject)

        If prApplication.Models.FindAll(Function(x) x.IsDirty = True).Count > 0 Then
            Dim lsMessage As String = ""
            lsMessage = "The following Models require saving before changing Project:"
            lsMessage &= vbCrLf

            For Each lrModel In prApplication.Models.FindAll(Function(x) x.IsDirty = True)
                lsMessage &= vbCrLf & lrModel.Name
            Next

            MsgBox(lsMessage)

            Me.ComboBoxProject.SelectedIndex = Me.ziSelectedProjectIndex

        Else
            Call Me.clearModels()
        End If



    End Sub

    Private Sub ComboBoxProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxProject.SelectedIndexChanged

        Try
            If (Not Me.ComboBoxProject.SelectedIndex = -1) _
                And Not Me.zbLoadingProjects Then

                'Clear the Page Nodes
                prPageNodes.Clear()

                Me.ComboBoxNamespace.Items.Clear()
                Call Me.removeAllModelsFromTreeView()

                Dim lrProject As ClientServer.Project = Me.ComboBoxProject.SelectedItem.Tag
                Call Me.loadNamespacesForProject(lrProject)
                Me.zrProject = lrProject

                '----------------------------------------------------------------------------
                'Set the WorkingProject of the Application.
                prApplication.WorkingProject = lrProject

                '----------------------------------------------------------------------------
                'Get the User Permissions for the Project
                If prApplication.User IsNot Nothing Then
                    If Me.ComboBoxProject.SelectedItem.Tag.Id = "MyPersonalModels" Then
                        prApplication.User.ProjectPermission.Clear()
                        Dim lrPermission As New ClientServer.Permission(lrProject, pcenumPermissionClass.User, pcenumPermission.FullRights)
                        prApplication.User.ProjectPermission.Add(lrPermission)

                        lrPermission = New ClientServer.Permission(lrProject, pcenumPermissionClass.User, pcenumPermission.Create)
                        prApplication.User.ProjectPermission.Add(lrPermission)

                        lrPermission = New ClientServer.Permission(lrProject, pcenumPermissionClass.User, pcenumPermission.Read)
                        prApplication.User.ProjectPermission.Add(lrPermission)

                        lrPermission = New ClientServer.Permission(lrProject, pcenumPermissionClass.User, pcenumPermission.Alter)
                        prApplication.User.ProjectPermission.Add(lrPermission)
                    Else
                        Call prApplication.User.getProjectPermissions(lrProject)
                    End If
                End If

                '---------------------------------------------------------------------------
                'Load the Models for the Project
                If Me.ComboBoxProject.SelectedItem.Tag.Id = "MyPersonalModels" And prApplication.User IsNot Nothing Then
                    Call Me.LoadModels(prApplication.User.Id, Nothing)
                Else
                    Me.zsNamespaceId = Me.ComboBoxNamespace.SelectedItem.ItemData
                    Call Me.LoadModels(Nothing, Me.zsNamespaceId)
                End If


                If Me.TreeView.Nodes(0).Nodes.Count = 1 Then
                    Me.TreeView.Nodes(0).Expand()
                End If

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Removes all the Models from the TreeView. e.g. When selecting a Project when under Client/Server mode.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub removeAllModelsFromTreeView()

        Try

            Dim liInd As Integer = 0
            Dim lrNode As TreeNode

            Me.zbRemovingModels = True
            prApplication.Models.Clear()
            For liInd = Me.TreeView.Nodes(0).Nodes.Count - 1 To 0 Step -1
                lrNode = Me.TreeView.Nodes(0).Nodes(liInd)
                Me.TreeView.SelectedNode = lrNode
                lrNode.Remove()
            Next
            Me.zbRemovingModels = False

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub loadUserToJoinGroupInvitations(Optional ByVal asGroupId As String = Nothing)

        Dim loNB As GroupNotificationBar
        Dim larInvitations As New List(Of ClientServer.Invitation)
        Dim lrGroup As ClientServer.Group

        larInvitations = tableClientServerInvitation.getOpenInvitationsForUserByType(prApplication.User, pcenumInvitationType.UserToJoinGroup)

        For Each lrInvitation In larInvitations

            lrGroup = New ClientServer.Group
            Call tableClientServerGroup.getGroupDetailsById(Trim(lrInvitation.Tag), lrGroup)

            If asGroupId IsNot Nothing Then
                If lrGroup.Id = asGroupId Then
                    loNB = New GroupNotificationBar
                    loNB.Name = "informationBar1"
                    loNB.Size = New Size(100, 40)
                    loNB.Dock = DockStyle.Bottom
                    loNB.Font = New Font(FontFamily.GenericSansSerif, 8.25F)
                    loNB.BackColor = Color.PaleGoldenrod
                    loNB.Text = "You've been invited to join the Group," & lrGroup.Name & ", by : " & lrInvitation.InvitingUser.Username
                    Me.TreeView.Controls.Add(loNB)
                    loNB.Left = 0
                    loNB.Show()
                    loNB.Tag = lrInvitation
                    loNB.Flash(200, 5)

                    AddHandler loNB.AcceptPressed, AddressOf AcceptGroupInvitation
                    AddHandler loNB.DeclinePressed, AddressOf DeclineGroupInvitation
                End If
            Else
                loNB = New GroupNotificationBar
                loNB.Name = "informationBar1"
                loNB.Size = New Size(100, 40)
                loNB.Dock = DockStyle.Bottom
                loNB.Font = New Font(FontFamily.GenericSansSerif, 8.25F)
                loNB.BackColor = Color.PaleGoldenrod
                loNB.Text = "You've been invited to join the Group," & lrGroup.Name & ", by : " & lrInvitation.InvitingUser.Username
                Me.TreeView.Controls.Add(loNB)
                loNB.Left = 0
                loNB.Show()
                loNB.Tag = lrInvitation
                loNB.Flash(200, 5)

                AddHandler loNB.AcceptPressed, AddressOf AcceptGroupInvitation
                AddHandler loNB.DeclinePressed, AddressOf DeclineGroupInvitation
            End If

        Next

    End Sub

    Public Sub loadUserToJoinProjectInvitations(Optional asProjectId As String = Nothing)

        Dim loNB As Viev.NotificationBar
        Dim larInvitations As New List(Of ClientServer.Invitation)
        Dim lrProject As ClientServer.Project

        larInvitations = tableClientServerInvitation.getOpenInvitationsForUserByType(prApplication.User, pcenumInvitationType.UserToJoinProject)

        For Each lrInvitation In larInvitations

            lrProject = New ClientServer.Project
            Call tableClientServerProject.getProjectDetailsById(Trim(lrInvitation.Tag), lrProject)

            If asProjectId IsNot Nothing Then
                If lrProject.Id = asProjectId Then
                    loNB = New Viev.NotificationBar
                    loNB.Name = "informationBar1"
                    loNB.Size = New Size(100, 40)
                    loNB.Dock = DockStyle.Bottom
                    loNB.Font = New Font(FontFamily.GenericSansSerif, 8.25F)
                    loNB.Text = "You've been invited to join Project," & lrProject.Name & ", by : " & lrInvitation.InvitingUser.Username
                    Me.TreeView.Controls.Add(loNB)
                    loNB.Left = 0
                    loNB.Show()
                    loNB.Tag = lrInvitation
                    loNB.Flash(200, 5)

                    AddHandler loNB.AcceptPressed, AddressOf InvitationAcceptedButton_Click
                    AddHandler loNB.DeclinePressed, AddressOf InvitationDeclinedButton_Click
                End If
            Else
                loNB = New Viev.NotificationBar
                loNB.Name = "informationBar1"
                loNB.Size = New Size(100, 40)
                loNB.Dock = DockStyle.Bottom
                loNB.Font = New Font(FontFamily.GenericSansSerif, 8.25F)
                loNB.Text = "You've been invited to join Project," & lrProject.Name & ", by : " & lrInvitation.InvitingUser.Username
                Me.TreeView.Controls.Add(loNB)
                loNB.Left = 0
                loNB.Show()
                loNB.Tag = lrInvitation
                loNB.Flash(200, 5)

                AddHandler loNB.AcceptPressed, AddressOf InvitationAcceptedButton_Click
                AddHandler loNB.DeclinePressed, AddressOf InvitationDeclinedButton_Click
            End If

        Next

    End Sub

    Private Sub InvitationAcceptedButton_Click(ByVal arInvitation As Object)

        Dim lrInvitation As New ClientServer.Invitation

        lrInvitation = CType(arInvitation, ClientServer.Invitation)

        lrInvitation.Accepted = True
        lrInvitation.Closed = True

        Dim lrProject As New ClientServer.Project

        Call tableClientServerProject.getProjectDetailsById(lrInvitation.Tag, lrProject)

        '-----------------
        Dim lrComboboxItem As tComboboxItem

        Me.zbLoadingProjects = True 'So that changing the Index item doesn't trigger loading Namespaces for each Project loaded

        lrComboboxItem = New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
        Me.ComboBoxProject.Items.Add(lrComboboxItem)

        Me.zbLoadingProjects = False 'So that changing the Index item doesn't trigger loading Namespaces for each Project loaded
        '-----------------

        Call tableClientServerProjectUser.AddUserToProject(prApplication.User, lrProject)

        Dim lrRole As New ClientServer.Role("Modeller", "Modeller")
        Call tableClientServerProjectUserRole.AddUserRoleToProject(prApplication.User, lrRole, lrProject)

        Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission
        lrProjectUserPermission.Project = lrProject
        lrProjectUserPermission.User = prApplication.User
        lrProjectUserPermission.Permission = pcenumPermission.FullRights
        Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
        lrProjectUserPermission.Permission = pcenumPermission.Create
        Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
        lrProjectUserPermission.Permission = pcenumPermission.Read
        Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
        lrProjectUserPermission.Permission = pcenumPermission.Alter
        Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)

        Call tableClientServerInvitation.closeInvitation(lrInvitation) 'Key: InvitationType, DateTime, InvitingUser, InvitedUser

    End Sub

    Private Sub AcceptGroupInvitation(ByVal arInvitation As Object)

        Dim lrInvitation As New ClientServer.Invitation

        lrInvitation = CType(arInvitation, ClientServer.Invitation)

        lrInvitation.Accepted = True
        lrInvitation.Closed = True

        Dim lrGroup As New ClientServer.Group

        Call tableClientServerGroup.getGroupDetailsById(lrInvitation.Tag, lrGroup)

        ''-----------------
        'Dim lrComboboxItem As tComboboxItem

        'Me.zbLoadingProjects = True 'So that changing the Index item doesn't trigger loading Namespaces for each Project loaded

        'lrComboboxItem = New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
        'Me.ComboBoxProject.Items.Add(lrComboboxItem)

        'Me.zbLoadingProjects = False 'So that changing the Index item doesn't trigger loading Namespaces for each Project loaded
        ''-----------------

        Call tableClientServerGroupUser.AddUserToGroup(prApplication.User, lrGroup)

        Call tableClientServerInvitation.closeInvitation(lrInvitation) 'Key: InvitationType, DateTime, InvitingUser, InvitedUser

    End Sub

    Private Sub InvitationDeclinedButton_Click(ByVal arInvitation As Object)

        Dim lrInvitation As New ClientServer.Invitation

        lrInvitation = CType(arInvitation, ClientServer.Invitation)

        lrInvitation.Accepted = False
        lrInvitation.Closed = True

        Call tableClientServerInvitation.closeInvitation(lrInvitation) 'Key: InvitationType, DateTime, InvitingUser, InvitedUser

    End Sub

    Private Sub DeclineGroupInvitation(ByVal arInvitation As Object)

        Dim lrInvitation As New ClientServer.Invitation

        lrInvitation = CType(arInvitation, ClientServer.Invitation)

        lrInvitation.Accepted = False
        lrInvitation.Closed = True

        Call tableClientServerInvitation.closeInvitation(lrInvitation) 'Key: InvitationType, DateTime, InvitingUser, InvitedUser

    End Sub

    Private Sub TocqlFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TocqlFileToolStripMenuItem.Click

        Dim lsFolderLocation As String = ""
        Dim lsFileName As String = ""
        'Dim loStreamWriter As StreamWriter ' Create file by FileStream class
        'Dim loXMLSerialiser As XmlSerializer ' Create binary object
        Dim lrModel As FBM.Model
        Dim lrExportModel As New XMLModel.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            lrExportModel.ORMModel.ModelId = lrModel.ModelId
            lrExportModel.ORMModel.Name = lrModel.Name

            Call lrExportModel.MapFromFBMModel(lrModel)

            Dim lsFileLocationName As String = ""
            If Boston.IsSerializable(lrExportModel) Then

                If My.Settings.UseClientServer And My.Settings.UseVirtualUI Then
                    lsFolderLocation = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData
                Else
                    If DialogFolderBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        lsFolderLocation = DialogFolderBrowser.SelectedPath
                    Else
                        Exit Sub
                    End If
                End If

                If My.Settings.UseClientServer And (prApplication.User IsNot Nothing) And My.Settings.UseVirtualUI Then
                    lsFileName = prApplication.User.Id & "-" & lrModel.Name & ".cql"
                    lsFileLocationName = lsFolderLocation & "\" & lsFileName
                Else
                    lsFileName = lrModel.Name & ".cql"
                    lsFileLocationName = lsFolderLocation & "\" & lsFileName
                End If

                '============================================================
                Dim file As System.IO.StreamWriter
                file = My.Computer.FileSystem.OpenTextFileWriter(lsFolderLocation & "\" & lsFileName, False)
                file.WriteLine("vocabulary " & lrModel.Name & ";" & vbCrLf)

                file.WriteLine(CQL.GetCommentCQL("Value Types"))
                For Each lrValueType In lrModel.ValueType
                    Call file.WriteLine(lrValueType.GetCQLText)
                Next
                Call file.WriteLine(vbCrLf)

                file.WriteLine(CQL.GetCommentCQL("Entity Types"))
                For Each lrEntityType In lrModel.EntityType
                    Call file.WriteLine(lrEntityType.GetCQLText)
                Next
                Call file.WriteLine(vbCrLf)

                file.WriteLine(CQL.GetCommentCQL("Fact Types"))
                For Each lrFactType In lrModel.FactType.FindAll(Function(x) x.IsUsedInReferenceScheme = False)
                    Call file.WriteLine(lrFactType.GetCQLText)
                Next
                Call file.WriteLine(vbCrLf)

                file.WriteLine(CQL.GetCommentCQL("Constraints"))
                For Each lrRoleConstraint In lrModel.RoleConstraint
                    Call file.WriteLine(lrRoleConstraint.GetCQLText)
                Next
                Call file.WriteLine(vbCrLf)

                file.Close()
                '============================================================

                'loStreamWriter = New StreamWriter(lsFolderLocation & "\" & lsFileName)

                ''loXMLSerialiser = New XmlSerializer(GetType(FBM.tORMModel))
                'loXMLSerialiser = New XmlSerializer(GetType(XMLModel.Model))

                '' Serialize object to file
                'loXMLSerialiser.Serialize(loStreamWriter, lrExportModel)
                'loStreamWriter.Close()

                If My.Settings.UseClientServer And My.Settings.UseVirtualUI Then
                    prThinfinity.DownloadFile(lsFileLocationName)
                End If

                Dim lsMessage As String = ""
                lsMessage = "Your file is ready for viewing."
                If Not My.Settings.UseClientServer Then
                    lsMessage &= vbCrLf & vbCrLf
                    lsMessage &= lsFileLocationName
                End If

                MsgBox(lsMessage)

            End If 'IsSerialisable

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxNamespace_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxNamespace.SelectedIndexChanged

        prApplication.WorkingNamespace = Me.ComboBoxNamespace.SelectedItem.Tag

    End Sub

    Private Sub TreeView_DragDrop(sender As Object, e As DragEventArgs) Handles TreeView.DragDrop

        Dim lsMessage As String = ""

        Me.zbDraggingOver = False

        If e.Effect = DragDropEffects.Copy Then

            If e.Data.GetDataPresent("Boston.cTreeNode", True) Then

                Dim dropNode As TreeNode = CType(e.Data.GetData("Boston.cTreeNode"), TreeNode)

                Select Case dropNode.Tag.Tag.GetType
                    Case Is = GetType(FBM.Model)

                        Dim pt As Point = sender.PointToClient(New Point(e.X, e.Y))
                        Dim DestinationNode As TreeNode = sender.GetNodeAt(pt)

                        If DestinationNode.Tag.Tag.GetType Is GetType(FBM.Model) Then

                            Dim lrDragModel As FBM.Model = dropNode.Tag.Tag
                            Dim lrDestinationModel As FBM.Model = DestinationNode.Tag.Tag

                            If lrDragModel.ModelId <> lrDestinationModel.ModelId Then

                                If Not MsgBox("Are you sure you want to merge the Model with the selected Model?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                                    Exit Sub
                                End If

                                If Not lrDestinationModel.Loaded Then
                                    Call lrDestinationModel.Load(abDontUseBLOBLoading:=True)
                                End If

#Region "Merge Models"
                                Dim lrMergeValueType As FBM.ValueType
                                For Each lrValueType In lrDragModel.ValueType.FindAll(Function(x) Not x.IsMDAModelElement)
                                    If lrDestinationModel.GetModelObjectByName(lrValueType.Id, True,, False) Is Nothing Then
                                        lrMergeValueType = lrValueType.Clone(lrDestinationModel, False, False)
                                        lrDestinationModel.AddValueType(lrMergeValueType, True, True, Nothing, True)
                                    End If
                                Next

                                Dim lrMergeEntityType As FBM.EntityType
                                For Each lrEntityType In lrDragModel.EntityType.FindAll(Function(x) Not x.IsMDAModelElement)
                                    If lrDestinationModel.GetModelObjectByName(lrEntityType.Id, True,, False) Is Nothing Then
                                        lrMergeEntityType = lrEntityType.Clone(lrDestinationModel, False, False)
                                        lrDestinationModel.AddEntityType(lrMergeEntityType, True, True, Nothing, True)
                                    End If
                                Next

                                Dim lrMergeFactType As FBM.FactType
                                For Each lrFactType In lrDragModel.FactType.FindAll(Function(x) Not x.IsMDAModelElement)
                                    If lrDestinationModel.GetModelObjectByName(lrFactType.Id, True,, False) Is Nothing Then
                                        lrMergeFactType = lrFactType.Clone(lrDestinationModel, False, False)
                                        lrDestinationModel.AddFactType(lrMergeFactType, True, True, Nothing)
                                    End If
                                Next

                                Dim lrMergeRoleConstraint As FBM.RoleConstraint
                                For Each lrRoleConstraint In lrDragModel.RoleConstraint.FindAll(Function(x) Not x.IsMDAModelElement)
                                    If lrDestinationModel.GetModelObjectByName(lrRoleConstraint.Id, True,, False) Is Nothing Then
                                        lrMergeRoleConstraint = lrRoleConstraint.Clone(lrDestinationModel, False, False)
                                        lrDestinationModel.AddRoleConstraint(lrMergeRoleConstraint, True, True, Nothing, False, Nothing, False)
                                    End If
                                Next

                                For Each lrGeneralConceptDictionaryEntry In lrDragModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept)

                                    Dim lrNewGeneralConceptDictionaryEntry = lrGeneralConceptDictionaryEntry.Clone(lrDestinationModel)
                                    Call lrDragModel.AddModelDictionaryEntry(lrNewGeneralConceptDictionaryEntry, True, True, False, False, False, True, False, False)

                                Next
#End Region

                            Else
                                MsgBox("You can't merge a Model with itself.")
                                Exit Sub
                            End If

                        Else
                            MsgBox("You can only merge a Model with a Model. Drop on a Model.")
                            Exit Sub
                        End If

                    Case Is = GetType(FBM.Page)

                        If Not MsgBox("Are you sure you want to merge the Page with the selected Model?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                            Exit Sub
                        End If

                        Dim lrPage, lrNewPage As FBM.Page

                        lrPage = dropNode.Tag.Tag

                        Select Case lrPage.Language
                            Case Is = pcenumLanguage.EntityRelationshipDiagram,
                                      pcenumLanguage.PropertyGraphSchema
                                lsMessage = "You can only copy ORM Diagram Pages from one Model to another Model."
                                MsgBox(lsMessage)
                                Exit Sub
                        End Select

                        Dim pt As Point = sender.PointToClient(New Point(e.X, e.Y))
                        Dim DestinationNode As TreeNode = sender.GetNodeAt(pt)

                        If DestinationNode.Tag.Tag.GetType Is GetType(FBM.Model) Then

                            Dim lrModel As FBM.Model = DestinationNode.Tag.Tag

                            If lrPage.Model.ModelId <> lrModel.ModelId Then

                                If Not lrModel.Loaded Then
                                    Call lrModel.Load(abDontUseBLOBLoading:=True)
                                End If

                                lrPage.Name = lrModel.CreateUniquePageName(lrPage.Name, 0)

                                lrNewPage = lrPage.Clone(lrModel, True, False, , True)
                                lrModel.Page.Add(lrNewPage)
                                Call lrModel.Save()

                                Call Me.AddExistingPageToModel(lrNewPage, lrNewPage.Model, DestinationNode, True)
                            Else
                                MsgBox("You cannot copy a Page to the Model that it belongs to.")
                            End If
                        End If
                End Select

            End If
        ElseIf e.Effect = DragDropEffects.Move Then
            lsMessage = "You cannot move Pages between Models in Boston. To copy a Page from one Model to another, hold down the [Ctrl] key while dragging the Page from one Model to another."
            MsgBox(lsMessage)
        End If

    End Sub

    Private Sub GenerateDocumentationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenerateDocumentationToolStripMenuItem.Click

        Try
            With New WaitCursor

                Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag

                prApplication.WorkingModel = lrModel

                If Not lrModel.Loaded Then
                    Call Me.DoModelLoading(lrModel)
                    Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
                End If

                While (prApplication.WorkingModel.Loading And Not prApplication.WorkingModel.Loaded) Or prApplication.WorkingModel.Page.FindAll(Function(x) x.Loading).Count > 0
                End While

                Dim rd As Gios.Word.WordDocument = New WordDocument(WordDocumentFormat.Letter_8_5x11)
                Dim lrFBMWordVerbaliser As New FBM.ORMWordVerbailser(rd, Me)

                Dim lrFrmGetDocumentGenerationSettings As New frmDocumentGeneratorSettings
                lrFrmGetDocumentGenerationSettings.zrORMWordVerbaliser = lrFBMWordVerbaliser

                If lrFrmGetDocumentGenerationSettings.ShowDialog = Windows.Forms.DialogResult.OK Then

                    Dim lrWordDocument As WordDocument = Nothing
                    Using lrWaitCursor As New WaitCursor
                        lrWordDocument = lrFBMWordVerbaliser.createWordDocument(prApplication.WorkingModel)
                    End Using

                    If lrWordDocument IsNot Nothing Then
                        'Create the RTF/Word Document
                        Dim lsFileName As String = ""
                        Try
                            Dim saveFileDialog As New SaveFileDialog

                            saveFileDialog.Filter = "Word Document (*.doc)|*.doc"
                            saveFileDialog.Title = "Save model documentation"
                            saveFileDialog.InitialDirectory = "C:\"
                            saveFileDialog.FilterIndex = 0
                            saveFileDialog.CheckFileExists = False
                            saveFileDialog.CheckPathExists = False
                            saveFileDialog.CreatePrompt = False

                            If saveFileDialog.ShowDialog(frmMain) = DialogResult.OK Then

                                If saveFileDialog.FileName <> "" Then
                                    Try

                                        lsFileName = saveFileDialog.FileName
                                        lrWordDocument.SaveToFile(saveFileDialog.FileName) '"..\\..\\Example1.doc")

                                        If lsFileName <> "" Then
                                            System.Diagnostics.Process.Start(lsFileName) ' "..\..\Example1.doc")
                                        End If
                                    Catch ex As Exception
                                        MsgBox("Couldn't open the file.")
                                    End Try
                                End If
                            End If

                        Catch ex As Exception
                            MsgBox("Couldn't save the document. Check to see if you already have the document open.")
                        End Try
                    End If

                End If
            End With
        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        End Try

    End Sub

    Private Sub ContextMenuStrip_ORMModel_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ORMModel.Opening

        Me.ContextMenuStrip_ORMModel.ImageScalingSize = New Drawing.Size(16, 16)

        If prApplication.User IsNot Nothing Then 'Is nothing if not using Client/Server.
            If prApplication.User.IsSuperuser Or
               prApplication.User.Function.Contains(pcenumFunction.FullPermission) Or
               prApplication.User.Role.FindAll(Function(x) x.Name = "Superuser").Count > 0 Then
                Me.ToolStripMenuItemModelConfiguration.Enabled = True
            Else
                Me.ToolStripMenuItemModelConfiguration.Enabled = False
            End If
        End If

        If My.Settings.SuperuserMode = True Then
            Me.ToolStripMenuItemFixModelErrors.Visible = True
            Me.ToolStripMenuItemEmptyModel.Visible = True
        End If

    End Sub

    Private Sub TreeView_DragLeave(sender As Object, e As EventArgs) Handles TreeView.DragLeave

        Me.zbDraggingOver = False

    End Sub

    Private Sub AddSTDPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddSTDPageToolStripMenuItem.Click

        Try
            '========================================================================================
            'Can only add STDiagrams to Models that have at least one ValueType.
            Dim lrModel As FBM.Model
            lrModel = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            If lrModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False).Count = 0 Then
                Dim lsMessage = "The Model must have at least one Value Type before adding a State Transition Diagram"
                MsgBox(lsMessage)
                Exit Sub
            End If

            '==============================================================
            'Get the Core Metamodel.Page for an StateTransitionDiagram
            '==============================================================
            '-------------------------------------------
            'Get the StateTransitionDiagram Core Page
            '-------------------------------------------
            Dim lrPage As FBM.Page

            Using lrWaitCursor As New WaitCursor
                Boston.WriteToStatusBar("Loading the MetaModel for State Transition Diagrams")

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString,
                                               pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreStateTransitionDiagram.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the StateTransitionDiagram.
                '----------------------------------------------------
                Boston.WriteToStatusBar("Creating the Page.")
                lrPage = lrCorePage.Clone(prApplication.WorkingModel, True, True, , True) 'Assigns new PageId
                lrPage.Name = prApplication.WorkingModel.CreateUniquePageName("NewStateTransitionDiagram", 0)
                lrPage.Language = pcenumLanguage.StateTransitionDiagram

                Call Me.AddPageToModel(Me.TreeView.SelectedNode, lrPage, False)

                Call lrPage.Save()

            End Using 'WaitCursor

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RenameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem.Click

        Call Me.TreeView.SelectedNode.BeginEdit()

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click

        Call Me.TreeView.SelectedNode.BeginEdit()

    End Sub

    Private Sub ButtonNewModel_Click(sender As Object, e As EventArgs) Handles ButtonNewModel.Click

        With New WaitCursor
            Call Me.addNewModelToBoston()
        End With

    End Sub

    Private Sub zrToolTip_Draw(sender As Object, e As DrawToolTipEventArgs) Handles zrToolTip.Draw

        'e.DrawText()

        Dim b As System.Drawing.Drawing2D.LinearGradientBrush = New System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.GreenYellow, Color.MintCream, 45.0F)

        e.Graphics.FillRectangle(b, e.Bounds)

        e.Graphics.DrawRectangle(New Pen(Brushes.Red, 1), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 100, e.Bounds.Height - 100))
        e.Graphics.FillRectangle(SystemBrushes.Info, e.Bounds)

        'e.DrawBackground()
        'e.DrawBorder()

        Dim f As Font = New Font("Arial", 12)
        e.Graphics.DrawString(e.ToolTipText, f, Brushes.Black, New PointF(1, 1))

    End Sub

    Private Sub frmToolboxEnterpriseExplorer_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd

        Me.TreeView.Left = 3
    End Sub

    Private Sub CodeGenerationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemCodeGenerator.Click

        If prApplication.SoftwareCategory = pcenumSoftwareCategory.Student Then
            Dim lsMessage = "Please upgrade to Boston Professional for code generation."
            MsgBox(lsMessage)
        Else
            With New WaitCursor
                Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
                If Not lrModel.Loaded Then
                    Call Me.DoModelLoading(lrModel)
                    Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
                End If

                Call frmMain.loadCodeGenerator()
            End With
        End If


    End Sub

    Private Sub FactEngineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FactEngineToolStripMenuItem.Click
        With New WaitCursor
            Try
                Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
                If Not lrModel.Loaded Then
                    Call Me.DoModelLoading(lrModel)
                    Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
                End If

                Call frmMain.LoadFactEngine()
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End With
    End Sub

    Private Sub HideToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideToolStripMenuItem.Click
        Me.TreeView.SelectedNode.Hidden(False) = True
    End Sub

    Private Sub UnhideHiddenModelsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnhideHiddenModelsToolStripMenuItem.Click
        Me.TreeView.SelectedNode.Hidden(False, True) = False
    End Sub

    Private Sub TreeView_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView.BeforeSelect
        Me.TreeView._SelectedNode = e.Node
    End Sub

    Private Sub TreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView.NodeMouseClick
        If e.Button = MouseButtons.Right Then
            Me.ziMouseButton = e.Button
            Me.TreeView.SelectedNode = e.Node

        End If
    End Sub

    Private Sub HideAllotherModelsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideAllotherModelsToolStripMenuItem.Click

        Try

            For Each lrNode As cTreeNode In Me.TreeView.Nodes(0).Nodes
                If lrNode IsNot Me.TreeView.SelectedNode Then
                    lrNode.Hidden(False, False) = True
                End If
            Next

            Me.TreeView.SelectedNode.EnsureVisible()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemFixModelErrors_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFixModelErrors.Click

        Dim lrModel As FBM.Model

        Try

            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            Call frmMain.LoadFixModelErrorsForm(lrModel)

        Catch ex As Exception
        End Try
    End Sub

    Private Sub ContextMenuStrip_ORMModels_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ORMModels.Opening

        Me.ContextMenuStrip_ORMModels.ImageScalingSize = New Drawing.Size(16, 16)

    End Sub

    Private Sub UnhideASelectedModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnhideASelectedModelToolStripMenuItem.Click

        Try
            Dim lfrmGenericSelect As New frmGenericSelect

            lfrmGenericSelect.zoGenericSelection.Type = pcenumGenericSelectionType.SelectFromList
            lfrmGenericSelect.zoGenericSelection.FormTitle = "Model to unhide"

            For Each lrNode As cTreeNode In Me.TreeView.Nodes(0).Nodes
                If lrNode.Hidden Then
                    lfrmGenericSelect.zoGenericSelection.TupleList.Add(New tComboboxItem(Nothing, lrNode.Text, lrNode))
                End If
            Next

            If lfrmGenericSelect.ShowDialog = DialogResult.OK Then
                Dim lrNode As cTreeNode = lfrmGenericSelect.zoGenericSelection.SelectedTag
                lrNode.Hidden(False) = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ImportormNORMAFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportormNORMAFileToolStripMenuItem.Click

        Dim lsMessage As String = ""

        Try
            Me.DialogOpenFile.DefaultExt = "orm"
            Me.DialogOpenFile.Filter = "NORMA Files (*.orm)|*.orm"

            If Me.DialogOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                '=========================================================================================
                'ModelName/Id & Model
                Dim lsModelName As String = Path.GetFileNameWithoutExtension(Me.DialogOpenFile.FileName)

                '========================================================================================
                'XDocument - Open the document for reading.
                Dim loXDocument As XDocument = XDocument.Load(Me.DialogOpenFile.FileName)

                '=======================================================
                'Model Errors? Does the user wish to proceed?
                Dim lrNORMAFileLoader As New NORMA.NORMAXMLFileLoader(Nothing)
                If lrNORMAFileLoader.getModelErrorCount(loXDocument) > 0 Then
                    lsMessage = "The NORMA .orm file you are attempting to load contains modelling errors."
                    lsMessage.AppendDoubleLineBreak("Do you wish to proceed?")
                    lsMessage.AppendDoubleLineBreak("If you decide to proceed Boston will do its best to compensate for those errors.")
                    If MsgBox(lsMessage, MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.No Then
                        Exit Sub
                    End If
                End If

                '==================================================================
                'Create the Model
                Dim lrModel As New FBM.Model(lsModelName, lsModelName)

                'Project 
                If prApplication.WorkingProject Is Nothing Then prApplication.WorkingProject = New ClientServer.Project("MyPersonalModels", "MyPersonalModels")
                lrModel.ProjectId = prApplication.WorkingProject.Id

                'Namespace
                If prApplication.WorkingProject.Id = "MyPersonalModels" Then
                    lrModel.Namespace = Nothing
                Else
                    lrModel.Namespace = Me.ComboBoxNamespace.SelectedItem.Tag
                End If

                If My.Settings.UseClientServer And (prApplication.User IsNot Nothing) Then
                    lrModel.CreatedByUserId = prApplication.User.Id
                End If

                Call lrModel.AddCoreERDPGSSTMUMLModelElements(Nothing)

                If TableModel.ExistsModelById(lrModel.ModelId) Or lrModel.ModelId.Length > 49 Then
                    lrModel.ModelId = System.Guid.NewGuid.ToString
                End If

                If TableModel.ExistsModelByName(lrModel.Name) Then
                    lrModel.Name = lrModel.CreateUniqueModelName(lrModel.Name, 0)
                End If

                '========================================================================================
                'TreeNode
                Dim lrNewTreeNode = Me.AddModelToModelExplorer(lrModel, False)

                '========================================================================================
                'Load the NORMA file
                pbDoDatabaseProcessing = False
                Call Me.LoadNORMAXMLFile(lrModel, loXDocument, lrNewTreeNode)
                pbDoDatabaseProcessing = True

                '========================================================================================
                'Save the Model?
                Dim lrCustomMessageBox As New frmCustomMessageBox

                lsMessage = "Your NORMA Model has been successfully loaded into Boston." & vbCrLf & vbCrLf
                lsMessage &= "Save the model now? (Recommended)"

                lrCustomMessageBox.Message = lsMessage
                lrCustomMessageBox.ButtonText.Add("No")
                lrCustomMessageBox.ButtonText.Add("Save to database")
                lrCustomMessageBox.ButtonText.Add("Store as XML")

                Select Case lrCustomMessageBox.ShowDialog
                    Case Is = "Store as XML"
                        lrModel.StoreAsXML = True
                        Boston.WriteToStatusBar("Saving Model: " & lrModel.Name)
                        Call lrModel.Save(True, False)
                        Boston.WriteToStatusBar("Model Saved")
                    Case Is = "Save to database"
                        With New WaitCursor
                            Boston.WriteToStatusBar("Saving Model: " & lrModel.Name)
                            Call lrModel.Save(True, False)
                            Boston.WriteToStatusBar("Model Saved")
                        End With
                End Select

                Call lrNewTreeNode.EnsureVisible()

                'Me.zrToolTip.IsBalloon = True
                lsMessage = "New Model added: " & lrModel.Name
                Me.zrToolTip.IsBalloon = True
                Me.zrToolTip.ToolTipIcon = ToolTipIcon.None
                Me.zrToolTip.Show(lsMessage, Me, lrNewTreeNode.Bounds.X, lrNewTreeNode.Bounds.Y + lrNewTreeNode.Bounds.Height, 4000)
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub KeywordExtractionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemKeywordExtractionTool.Click

        Dim lrModel As FBM.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = New FBM.Model
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            'CodesSafe-Load the model
            If Not lrModel.Loaded Then
                With New WaitCursor
                    Call lrModel.Load(False, False, Nothing, False, abDontUseBLOBLoading:=True)
                End With
            End If

            Call frmMain.LoadKeywordExtractionTool(lrModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AddBPMNPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddBPMNPageToolStripMenuItem.Click

        Try
            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            '==============================================================
            'Get the Core Metamodel.Page for an EntityRelationshipDiagram
            ' NB Is the same metamodel as used for PropertyGraphSchemas
            '==============================================================
            '-------------------------------------------
            'Get the EntityRelationshipModel Core Page
            '-------------------------------------------
            Using lrWaitCursor As New WaitCursor
                Dim lrPage As FBM.Page

                Boston.WriteToStatusBar("Loading the MetaModel for Property Graph Schemas.")

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreBPMNDiagram.ToString,
                                               pcenumCMMLCorePage.CoreBPMNDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CorePropertyGraphSchema.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the EntityRelationshipDiagram.
                '----------------------------------------------------
                Boston.WriteToStatusBar("Creating the Page.")
                lrPage = lrCorePage.Clone(prApplication.WorkingModel)
                lrPage.Name = prApplication.WorkingModel.CreateUniquePageName("BPMN-NewPage", 0)
                lrPage.Language = pcenumLanguage.BPMNChoreographDiagram

                Call Me.AddPageToModel(Me.TreeView.SelectedNode, lrPage, False, True)

                Call lrPage.Save()

            End Using

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub ToolStripMenuItemTaxonomyTree_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemTaxonomyTree.Click

        Dim lrModel As FBM.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = New FBM.Model
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            'CodesSafe-Load the model
            If Not lrModel.Loaded Then
                With New WaitCursor
                    Call lrModel.Load(False, False, Nothing, False, abDontUseBLOBLoading:=True)
                End With
            End If

            Call frmMain.LoadToolboxTaxonomyTree(lrModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AddUseCaseDiagramToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddUseCaseDiagramToolStripMenuItem.Click

        Try
            '========================================================================================            
            Dim lrModel As FBM.Model
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            '==============================================================
            'Get the Core Metamodel.Page for an UseCaseDiagram
            '==============================================================
            Dim lrPage As FBM.Page

            Using lrWaitCursor As New WaitCursor
                Boston.WriteToStatusBar("Loading the MetaModel for Use Case Diagrams")

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString,
                                               pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the StateTransitionDiagram.
                '----------------------------------------------------
                Boston.WriteToStatusBar("Creating the Page.")
                lrPage = lrCorePage.Clone(prApplication.WorkingModel, True, True, , True) 'Assigns new PageId
                lrPage.Name = prApplication.WorkingModel.CreateUniquePageName("UML-UCD-NewDiagram", 0)
                lrPage.Language = pcenumLanguage.UMLUseCaseDiagram

                Call Me.AddPageToModel(Me.TreeView.SelectedNode, lrPage, False)

                Call lrPage.Save()

            End Using 'WaitCursor

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AddBPMNChoreographyDiagramPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddBPMNChoreographyDiagramPageToolStripMenuItem.Click



    End Sub

    Private Sub AddBPMNCollaborationDiagramPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddBPMNCollaborationDiagramPageToolStripMenuItem.Click

        Try
            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            '==============================================================
            'Get the Core Metamodel.Page for an EntityRelationshipDiagram
            ' NB Is the same metamodel as used for PropertyGraphSchemas
            '==============================================================
            '-------------------------------------------
            'Get the EntityRelationshipModel Core Page
            '-------------------------------------------
            Using lrWaitCursor As New WaitCursor
                Dim lrPage As FBM.Page

                Boston.WriteToStatusBar("Loading the MetaModel for Property Graph Schemas.")

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreBPMNDiagram.ToString,
                                               pcenumCMMLCorePage.CoreBPMNDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreBPMNDiagram.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the BPMNDiagram.
                '----------------------------------------------------
                Boston.WriteToStatusBar("Creating the Page.")
                lrPage = lrCorePage.Clone(prApplication.WorkingModel)
                lrPage.Name = prApplication.WorkingModel.CreateUniquePageName("BPMN-Collaboration-NewPage", 0)
                lrPage.Language = pcenumLanguage.BPMNCollaborationDiagram

                Call Me.AddPageToModel(Me.TreeView.SelectedNode, lrPage, False, True)

                Call lrPage.Save()

            End Using

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try




    End Sub

    Private Sub ToNORMAormFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemExportToNORMAormFile.Click

        Dim lsMessage As String

        Try
            Dim lrModel As FBM.Model
            Dim lrFBMModel As New XMLModel.Model
            Dim lrNORMADocument As NORMA.ORMDocument

            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            With New WaitCursor

                lrFBMModel.ORMModel.ModelId = lrModel.ModelId
                lrFBMModel.ORMModel.Name = lrModel.Name

                Dim lbExcludeMDAModelElements As Boolean = My.Settings.ExportFBMExcludeMDAModelElements

                If Not My.Settings.ExportFBMExcludeMDAModelElements Then
                    lsMessage = "Do you want exclude Boston Core MDA (Model Driven Architecture) elements? (recommended)"
                    If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        lbExcludeMDAModelElements = True
                    End If
                End If

                Boston.WriteToStatusBar("Converting the Model to the .fbm Fact-Based Model format")

                If Not lrFBMModel.MapFromFBMModel(lrModel, lbExcludeMDAModelElements) Then
                    MsgBox("Fix the model errors, then try again")
                    Exit Sub
                End If

                lrModel = Me.TreeView.SelectedNode.Tag.Tag
                If Not lrModel.Loaded Then
                    Call Me.DoModelLoading(lrModel)
                    Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
                End If

                Boston.WriteToStatusBar("Converting the Model to the .orm NORMA format")
                lrNORMADocument = lrFBMModel.MapToNORMAORMModel(New NORMA.ORMDocument, frmMain.BackgroundWorkerStatusBar)
                Boston.WriteToStatusBar("", True, 0)

                Dim lrSaveFileDialog As New SaveFileDialog()

                Dim lsFileLocationName = Path.GetFileNameWithoutExtension(Me.DialogOpenFile.FileName) 'Me.mrNORMADocument.ORMModel.Name & ".orm"

                lrSaveFileDialog.Filter = "NORMA ORM file (*.orm)|*.orm"
                lrSaveFileDialog.FilterIndex = 0
                'lrSaveFileDialog.InitialDirectory = Path.GetFullPath("..\..\..\TestOutputNORMAModels-OverwiteFilesHerein")
                lrSaveFileDialog.FileName = lsFileLocationName

                If lrSaveFileDialog.ShowDialog() = DialogResult.OK Then
                    lsFileLocationName = lrSaveFileDialog.FileName
                Else
                    Exit Sub
                End If

                Try
                    Call lrNORMADocument.SerializeObject(lrNORMADocument, lsFileLocationName)
                Catch ex As XmlSchemaException
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    Throw New Exception(ex.InnerException.Message & vbCrLf & vbCrLf & ex.Message)
                End Try

                If My.Settings.UseClientServer And My.Settings.UseVirtualUI Then
                    prThinfinity.DownloadFile(lsFileLocationName)
                End If

            End With

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemMoveModel_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemMoveModel.Click

        Try
            Dim lfrmModelMove As New frmModelMove
            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag
            lfrmModelMove.mrModel = lrModel

            If lfrmModelMove.ShowDialog() = DialogResult.OK Then

                If lfrmModelMove.mrProject.Id <> prApplication.WorkingProject.Id And
                   lfrmModelMove.mrNamespace.Id <> prApplication.WorkingNamespace.Id Then

                    lrModel.ProjectId = lfrmModelMove.mrProject.Id
                    lrModel.Namespace = lfrmModelMove.mrNamespace
                    lrModel.MakeDirty(False, False)
                    Call lrModel.Save()

                    Me.TreeView.Nodes.Remove(Me.TreeView.SelectedNode)

                End If

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub FEKLUploaderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FEKLUploaderToolStripMenuItem.Click

        Dim lrModel As FBM.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = New FBM.Model
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            'CodesSafe-Load the model
            If Not lrModel.Loaded Then
                With New WaitCursor
                    Call lrModel.Load(False, False, Nothing, False, abDontUseBLOBLoading:=True)
                End With
            End If

            Call frmMain.LoadFEKLUploaderTool(lrModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub EditAITrainingDataEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditAITrainingDataEditorToolStripMenuItem.Click

        Dim lsMessage As String

        Try
            If My.Settings.FactEngineUseGPT3 Then

                Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag

                If Not lrModel.Loaded Then
                    Call Me.DoModelLoading(lrModel)
                End If

                While (prApplication.WorkingModel.Loading And Not prApplication.WorkingModel.Loaded) Or prApplication.WorkingModel.Page.FindAll(Function(x) x.Loading).Count > 0
                    Boston.WriteToStatusBar("Still loading the Model's Pages")
                End While

                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)

                Call frmMain.LoadToolboxAIPretrainingDataEditor(lrModel)

            Else
                lsMessage = "Please check that your instance of Boston/FactEngine is set up for naural language queries using AI."
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True,,,)
            End If
        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SearchTextbox_TextBoxCleared() Handles SearchTextbox.TextBoxCleared

        Try
            Call Me.FindTreeNode(Me.TreeView, Nothing)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ViewDatabaseSchemaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewDatabaseSchemaToolStripMenuItem.Click

        Try
            Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Tag

            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            'CodeSafe
            If lrModel.TargetDatabaseType = pcenumDatabaseType.None Or lrModel.TargetDatabaseConnectionString Is Nothing Or lrModel.TargetDatabaseConnectionString = "" Then
                Exit Sub
            End If

            Dim lfrmToolboxDatabaseSchemaViewer As frmToolboxDatabaseSchemaViewer = frmMain.LoadToolboxDatabaseSchemaView
            lfrmToolboxDatabaseSchemaViewer.mrModel = lrModel

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub VirtualBusinessAnalystToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VirtualBusinessAnalystToolStripMenuItem.Click

        Try
            Dim lrModel As FBM.Model


            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Tag

            Call frmMain.LoadVirtualBusinessAnalyst(lrModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToFEKLtxtFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToFEKLtxtFileToolStripMenuItem.Click

        Dim lsFolderLocation As String = ""
        Dim lsFileName As String = ""
        Dim lrModel As FBM.Model
        Dim lrExportModel As New XMLModel.Model

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Tag
            If Not lrModel.Loaded Then
                Call Me.DoModelLoading(lrModel)
                Call Me.SetWorkingEnvironmentForObject(Me.TreeView.SelectedNode.Tag)
            End If

            lrExportModel.ORMModel.ModelId = lrModel.ModelId
            lrExportModel.ORMModel.Name = lrModel.Name

            If My.Settings.ExportFBMExcludeMDAModelElements Then
                If MsgBox("Important: Your configuration settings will only allow the export of Object-Role Models. Are you happy to proceed?", MsgBoxStyle.YesNoCancel) <> MsgBoxResult.Yes Then
                    Exit Sub
                End If
            End If

            Dim lsFileLocationName As String = ""

            If My.Settings.UseClientServer And My.Settings.UseVirtualUI Then
                lsFolderLocation = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData
                lsFileName = prApplication.User.Id & "-" & lrModel.Name & ".txt"
                lsFileLocationName = lsFolderLocation & "\" & lsFileName
            Else
                Dim lrSaveFileDialog As New SaveFileDialog()

                lsFileName = lrModel.Name & ".txt"
                lsFileLocationName = lsFileName

                lrSaveFileDialog.Filter = "FEKL Text File (*.txt)|*.txt"
                lrSaveFileDialog.FilterIndex = 0
                lrSaveFileDialog.RestoreDirectory = True
                lrSaveFileDialog.FileName = lsFileLocationName

                If lrSaveFileDialog.ShowDialog() = DialogResult.OK Then

                    Dim lsFEKLFileText = lrModel.GenerateFEKL

                    File.WriteAllText(lrSaveFileDialog.FileName, lsFEKLFileText)

                    Dim lsMessage = "Your file is ready for viewing."

                    Call Boston.ShowFlashCard(lsMessage, Color.DarkSeaGreen, 2500, 10)

                End If

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        End Try
    End Sub

    Private Sub FromRDFOWLTurtlettlFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FromRDFOWLTurtlettlFileToolStripMenuItem.Click

        Try
            Dim openFileDialog As New OpenFileDialog()

            ' Set the file filter to only show .ttl files
            openFileDialog.Filter = "RDF Turtle Files (*.ttl)|*.ttl"

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Dim filePath As String = openFileDialog.FileName

                ' Create a new RDF graph
                Dim graph As New Graph()

                ' Use the Turtle parser to load the file into the graph
                Try
                    Dim parser As New TurtleParser()
                    parser.Load(graph, filePath)

                    Dim outputLines As New List(Of String)
                    Dim lsRelation As String = ""
                    Dim larObjectTypeName As New List(Of String)
                    Dim larFactTypeName As New List(Of String)

                    ' Display or process the natural language triples with labels
                    For Each triple In graph.Triples

                        ' Get the subject, predicate, and object nodes
                        Dim lrSubject As INode = triple.Subject
                        Dim lrPredicate As INode = triple.Predicate
                        Dim lrObject As INode = triple.Object

                        ' Extract labels using the rdfs:label property
                        '================Subject=================
                        Dim lsSubjectLabel As String = GetRDFLabel(graph, lrSubject)
                        If Uri.IsWellFormedUriString(lsSubjectLabel, UriKind.Absolute) Then
                            lsSubjectLabel = New Uri(lsSubjectLabel).Segments.Last()
                        End If
                        '================Predicate===============
                        Dim lsPredicate As String = GetRDFLabel(graph, lrPredicate)
                        ' Get the local name of the predicate without the full URI
                        lsPredicate = New Uri(lsPredicate).Segments.Last()
                        '================Object==================
                        Dim lsObjectLabel As String = GetRDFLabel(graph, lrObject)
                        If Uri.IsWellFormedUriString(lsObjectLabel, UriKind.Absolute) Then
                            lsObjectLabel = New Uri(lsObjectLabel).Segments.Last()
                        End If

                        Dim lsSubjectLabelPascalCase = lsSubjectLabel.ToPascalCaseWithSpaces
                        Dim lsObjectLabelPascalCase = lsObjectLabel.ToPascalCaseWithSpaces

                        '===========Clean=============================
                        lsSubjectLabelPascalCase = CleanModelelementName(lsSubjectLabelPascalCase)
                        lsObjectLabelPascalCase = CleanModelelementName(lsObjectLabelPascalCase)
                        lsPredicate = CleanModelelementName(lsPredicate)

                        If lsSubjectLabel.StartsWith("SAMPLE") Then GoTo SkipTriple

                        Select Case lsPredicate
                            Case Is = "rdf-schema", "isProxy"
                                GoTo SkipTriple
                            Case Is = "identifier", "22-rdf-syntax-ns"
                                GoTo SkipTriple 'For now
                            Case Is = "hasSource", "hasTarget"
                                GoTo SkipTriple 'For now
                            Case Is = "hasGenesysValue", "hasGenesysValueType"
                                GoTo SkipTriple 'For now
                            Case Is = "description"
                                GoTo SkipTriple 'For now
                        End Select

                        Dim pattern As String = "(.*?)(\s>\s)(.*?)\s>\s(.*)"
                        Dim match As Match = Regex.Match(lsSubjectLabel, pattern)

                        'hasPart, <other> section. I.e. Triples that have hasPart as the predicate, or some <other> predicate.
                        If match.Success Then
                            'FactTypeReading
                            Dim part1 As String = match.Groups(1).Value.Trim().ToPascalCaseWithSpaces
                            Dim part2 As String = match.Groups(3).Value.Trim()
                            Dim part3 As String = match.Groups(4).Value.Trim().ToPascalCaseWithSpaces

                            Dim lsFactTypeName = $"{part1}{part2.ToPascalCase}{part3}".RemoveWhitespace
                            Dim lsFactTypeReading = $"{part1} {part2} {part3}"
                            lsFactTypeName.RemoveDoubleWhiteSpace.RemoveWhitespace

                            If Not larFactTypeName.Contains(lsFactTypeName) Then
                                outputLines.AddUnique(lsFactTypeReading)
                            End If
                        Else
                            If Not larObjectTypeName.Contains(lsSubjectLabelPascalCase) Then
                                lsRelation = $"{lsSubjectLabelPascalCase} IS AN ENTITY TYPE"
                                outputLines.AddUnique(lsRelation)
                                larObjectTypeName.AddUnique(lsSubjectLabelPascalCase)
                            Else
                                lsRelation = $"{lsSubjectLabelPascalCase} IS A CONCEPT"
                                If Not outputLines.Contains(lsRelation) Then
                                    outputLines.AddUnique(lsRelation)
                                End If
                            End If

                            If Not larObjectTypeName.Contains(lsObjectLabelPascalCase) Then
                                lsRelation = $"{lsObjectLabelPascalCase} IS A CONCEPT"
                                outputLines.AddUnique(lsRelation)
                                larObjectTypeName.AddUnique(lsObjectLabelPascalCase)
                            End If

                            Dim lsFactTypeName As String = $"{lsSubjectLabelPascalCase}Has{lsObjectLabelPascalCase}".RemoveWhitespace

                            If Not larFactTypeName.Contains(lsFactTypeName) Then
                                'Create the Fact Type Reading
                                Dim lsFactTypeReading = $"{lsSubjectLabelPascalCase} has ONE {lsObjectLabelPascalCase}"
                                outputLines.AddUnique(lsFactTypeReading)
                            End If

                        End If

SkipTriple:
                    Next

                    ' Save the output lines to a text file
                    Dim saveFileDialog As New SaveFileDialog()
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt"
                    If saveFileDialog.ShowDialog() = DialogResult.OK Then
                        Dim outputFilePath As String = saveFileDialog.FileName
                        System.IO.File.WriteAllLines(outputFilePath, outputLines)
                    End If

                    Dim textWithNewlines As String = String.Join(Environment.NewLine, outputLines)
                    Clipboard.SetText(textWithNewlines)

                    Boston.ShowFlashCard("RDF file imported successfully.", Color.MediumSeaGreen, 2500, 10)

                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function GetRDFLabel(graph As IGraph, node As INode) As String
        'Dim labelPredicateUri As Uri = New Uri("http://www.w3.org/2000/01/rdf-schema#label")
        'Dim labelPredicate As INode = graph.CreateUriNode(labelPredicateUri)
        'Dim labelTriple As Triple = graph.GetTriplesWithSubjectPredicate(node, labelPredicate).FirstOrDefault()

        'If labelTriple IsNot Nothing Then
        '    Return labelTriple.Object.ToString()
        'Else
        '    Return node.ToString()
        'End If
        Dim rdfs As String = "http://www.w3.org/2000/01/rdf-schema#"
        Dim labelPredicate As INode = graph.CreateUriNode(New Uri(rdfs & "label"))
        Dim labelTriple As Triple = graph.GetTriplesWithSubjectPredicate(node, labelPredicate).FirstOrDefault()

        If labelTriple IsNot Nothing Then
            Return labelTriple.Object.ToString()
        Else
            Return node.ToString()
        End If
    End Function

    Private Function CleanModelelementName(ByVal asModelElementName As String) As String

        Try
            Dim lsReturnString As String = asModelElementName

            lsReturnString = lsReturnString.Replace(".", "")
            lsReturnString = lsReturnString.Replace(" - ", " ")
            lsReturnString = lsReturnString.Replace("-", " ")
            lsReturnString = lsReturnString.Replace("&", "And")
            lsReturnString = lsReturnString.Replace("/", " ")
            lsReturnString = lsReturnString.Replace(",", "")
            lsReturnString = lsReturnString.Replace("_", " ")
            lsReturnString = lsReturnString.Replace("(", "")
            lsReturnString = lsReturnString.Replace(")", "")
            lsReturnString = lsReturnString.Replace(":", "")

            Return lsReturnString.Trim.RemoveDoubleWhiteSpace

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

End Class
