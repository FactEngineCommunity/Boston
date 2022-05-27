Imports System.Reflection

Public Class frmToolboxModelDictionary

    Public WithEvents zrORMModel As FBM.Model
    Public zrLoadedModel As FBM.Model = Nothing
    Public ziLoadedLanguage As pcenumLanguage
    Private WithEvents mrApplication As tApplication = prApplication

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmToolboxModelDictionary_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        prApplication.RightToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub


    Private Sub frmModelDictionaryLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

    End Sub

    Public Sub SetupForm()

        If IsSomething(prApplication.WorkingPage) Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

        If IsSomething(prApplication.WorkingModel) Then
            Me.zrORMModel = prApplication.WorkingModel
        End If

        If IsSomething(Me.zrORMModel) Then
            Me.LabelModelName.Text = Me.zrORMModel.Name
        End If

        Me.CheckBoxShowModelDictionary.Visible = My.Settings.SuperuserMode
        Me.CheckBoxShowCoreModelElements.Visible = My.Settings.SuperuserMode

    End Sub

    Private Sub frm_model_dictionary_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        If IsSomething(prApplication.WorkingPage) Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

    End Sub

    Public Sub LoadToolboxModelDictionary(ByVal ailanguage As pcenumLanguage, Optional ByVal abForceReload As Boolean = False)

        Try
            If IsSomething(Me.zrLoadedModel) And Not abForceReload Then
                If prApplication.WorkingPage Is Nothing Then Exit Sub
                If (Me.zrLoadedModel Is Me.zrORMModel) And (prApplication.WorkingPage.Language = Me.ziLoadedLanguage) Then
                    Exit Sub
                End If
            End If

            Me.TreeView1.Nodes.Clear()
            Me.Refresh()

            If IsSomething(prApplication.WorkingModel) Then
                Me.zrORMModel = prApplication.WorkingModel
            End If

            If IsSomething(Me.zrORMModel) Then
                Me.LabelModelName.Text = Me.zrORMModel.Name
            End If

            Dim liLanguage As pcenumLanguage = pcenumLanguage.ORMModel

            Select Case Me.ComboBox1.SelectedIndex
                Case Is = 0
                    liLanguage = pcenumLanguage.ORMModel
                Case Is = 1
                    liLanguage = pcenumLanguage.EntityRelationshipDiagram
                Case Is = 2
                    liLanguage = pcenumLanguage.PropertyGraphSchema
            End Select

            If ailanguage <> liLanguage Then
                liLanguage = ailanguage
            End If

            RemoveHandler Me.ComboBox1.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged
            Select Case liLanguage
                Case Is = pcenumLanguage.ORMModel
                    Call Me.LoadORMModelDictionary()
                    Me.ComboBox1.SelectedIndex = 0
                Case Is = pcenumLanguage.EntityRelationshipDiagram
                    Call Me.LoadERDModelDictionary()
                    Me.ComboBox1.SelectedIndex = 1
                Case Is = pcenumLanguage.PropertyGraphSchema
                    Call Me.LoadPGSModelDictionary()
                    Me.ComboBox1.SelectedIndex = 2
            End Select
            AddHandler Me.ComboBox1.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged

            Me.ziLoadedLanguage = ailanguage

        Catch ex As Exception
            '---------------------------------------------------------------
            'Ignore for now. For some reason this function gets called
            '  when the form is closing and has no access to the TreeView
            '---------------------------------------------------------------
        End Try

    End Sub

    Private Sub LoadERDModelDictionary()

        Try
            Dim loNode As New TreeNode

            loNode = Me.TreeView1.Nodes.Add("Entity", "Entity", 0, 0)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.pageERD, Nothing)

            If prApplication.WorkingModel Is Nothing Then
                Exit Sub
            End If

            Dim lrEntity As RDS.Table

            prApplication.WorkingModel.RDS.Table.Sort(AddressOf RDS.Table.CompareName)

            Dim loColumnNode As New TreeNode
            For Each lrEntity In prApplication.WorkingModel.RDS.Table
                'loNode = New TreeNode
                loNode = Me.TreeView1.Nodes("Entity").Nodes.Add("Entity" & lrEntity.Name, lrEntity.Name, 0, 0)
                loNode.Tag = lrEntity

                For Each lrAttribute In lrEntity.Column
                    loColumnNode = loNode.Nodes.Add("Attribute" & lrAttribute.Name, lrAttribute.Name, 14, 14)
                    loColumnNode.Tag = lrAttribute
                Next

            Next

            Me.TreeView1.Nodes("Entity").Expand()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub LoadORMModelDictionary()

        Dim loNode As TreeNode
        Dim loSubNode As TreeNode

        Try
            loNode = Me.TreeView1.Nodes.Add("ObjectType", "Object Type", 0, 0)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.modelORMModel, Nothing)

            loNode = Me.TreeView1.Nodes.Add("FactType", "Fact Type", 0, 0)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.modelORMModel, Nothing)

            loNode = Me.TreeView1.Nodes.Add("Constraints", "Constraints", 0, 0)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.modelORMModel, Nothing)

            If Me.CheckBoxShowModelDictionary.Checked Then
                loNode = Me.TreeView1.Nodes.Add("ModelDictionary", "Model Dictionary", 0, 0)
                loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.modelORMModel, Nothing)
            End If

            If prApplication.WorkingModel Is Nothing Then
                Exit Sub
            End If

            Dim lrEntityType As New FBM.EntityType
            Dim larEntityType As New List(Of FBM.EntityType)
            Call prApplication.WorkingModel.EntityType.Sort(AddressOf FBM.EntityType.CompareEntityTypeNames)
            If Me.CheckBoxShowCoreModelElements.Checked Then
                larEntityType = prApplication.WorkingModel.EntityType.FindAll(Function(x) Not x.IsObjectifyingEntityType)
            Else
                larEntityType = prApplication.WorkingModel.EntityType.FindAll(Function(x) Not (x.IsMDAModelElement = True Or x.IsObjectifyingEntityType))
            End If
            For Each lrEntityType In larEntityType
                loNode = New TreeNode
                If lrEntityType.IsDerived Then
                    loNode = Me.TreeView1.Nodes("ObjectType").Nodes.Add("EntityType" & lrEntityType.Name, lrEntityType.Name, 15, 15)
                Else
                    loNode = Me.TreeView1.Nodes("ObjectType").Nodes.Add("EntityType" & lrEntityType.Name, lrEntityType.Name, 0, 0)
                End If
                loNode.Tag = lrEntityType
            Next

            Dim lrValueType As New FBM.ValueType
            Dim larValueType As New List(Of FBM.ValueType)
            Call prApplication.WorkingModel.ValueType.Sort(AddressOf FBM.ValueType.CompareValueTypeNames)
            If Me.CheckBoxShowCoreModelElements.Checked Then
                larValueType = prApplication.WorkingModel.ValueType
            Else
                larValueType = prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            End If
            For Each lrValueType In larValueType
                loNode = New TreeNode
                loNode = Me.TreeView1.Nodes("ObjectType").Nodes.Add("ValueType" & lrValueType.Name, lrValueType.Name, 1, 1)
                loNode.Tag = lrValueType
            Next

            Dim larFactType As New List(Of FBM.FactType)
            Dim lrFactType As New FBM.FactType
            Call prApplication.WorkingModel.FactType.Sort(AddressOf FBM.FactType.CompareFactTypeNames)
            If Me.CheckBoxShowCoreModelElements.Checked Then
                larFactType = prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsLinkFactType = False)
            Else
                larFactType = prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsMDAModelElement = False And x.IsLinkFactType = False)
            End If
            For Each lrFactType In larFactType
                If lrFactType.IsObjectified Then
                    loNode = New TreeNode
                    loNode = Me.TreeView1.Nodes("ObjectType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 3, 3)
                    loNode.Tag = lrFactType

                    loNode = loNode.Nodes.Add("LinkFactTypes" & lrFactType.Name, "Implied Fact Types")
                    loNode.Tag = "LinkFactTypesHolderNode"
                Else
                    loNode = New TreeNode
                    If lrFactType.IsSubtypeRelationshipFactType Then
                        loNode = Me.TreeView1.Nodes("FactType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 17, 17)
                    ElseIf lrFactType.Arity = 1 Then
                        If lrFactType.IsDerived Then
                            loNode = Me.TreeView1.Nodes("FactType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 19, 19)
                        Else
                            loNode = Me.TreeView1.Nodes("FactType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 16, 16)
                        End If
                    Else
                        If lrFactType.IsDerived Then
                            loNode = Me.TreeView1.Nodes("FactType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 18, 18)
                        Else
                            loNode = Me.TreeView1.Nodes("FactType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 2, 2)
                        End If
                    End If
                    loNode.Tag = lrFactType
                End If
            Next

            For Each lrFactType In prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsLinkFactType = True)
                Dim lrObjectifiedFactType As FBM.ModelObject
                lrObjectifiedFactType = lrFactType.RoleGroup(0).JoinedORMObject
                loNode = New TreeNode
                If Me.TreeView1.Nodes("ObjectType").Nodes.Find("LinkFactTypes" & lrObjectifiedFactType.Id, True).Count > 0 Then
                    loNode = Me.TreeView1.Nodes("ObjectType").Nodes.Find("LinkFactTypes" & lrObjectifiedFactType.Id, True)(0).Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 2, 2)
                    loNode.Tag = lrFactType
                End If
            Next

            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim larRoleConstraint As New List(Of FBM.RoleConstraint)
            Dim liImageIndex As Integer = 0
            If Me.CheckBoxShowCoreModelElements.Checked Then
                larRoleConstraint = prApplication.WorkingModel.RoleConstraint
            Else
                larRoleConstraint = prApplication.WorkingModel.RoleConstraint.FindAll(Function(x) x.IsMDAModelElement = False)
            End If
            Call prApplication.WorkingModel.RoleConstraint.Sort(AddressOf FBM.RoleConstraint.CompareRoleConstraintNames)
            For Each lrRoleConstraint In larRoleConstraint
                loNode = New TreeNode

                Select Case lrRoleConstraint.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.ExclusionConstraint
                        liImageIndex = 4
                    Case Is = pcenumRoleConstraintType.EqualityConstraint
                        liImageIndex = 5
                    Case Is = pcenumRoleConstraintType.SubsetConstraint
                        liImageIndex = 6
                    Case Is = pcenumRoleConstraintType.InclusiveORConstraint
                        liImageIndex = 7
                    Case Is = pcenumRoleConstraintType.ExclusiveORConstraint
                        liImageIndex = 8
                    Case Is = pcenumRoleConstraintType.FrequencyConstraint
                        liImageIndex = 9
                    Case Is = pcenumRoleConstraintType.RingConstraint
                        liImageIndex = 10
                    Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                        If lrRoleConstraint.IsPreferredIdentifier Then
                            liImageIndex = 20
                        Else
                            liImageIndex = 11
                        End If
                    Case Is = pcenumRoleConstraintType.ValueComparisonConstraint
                        liImageIndex = 23
                    Case Else
                        liImageIndex = 2
                End Select
                loNode = Me.TreeView1.Nodes("Constraints").Nodes.Add("RoleConstraint" & lrRoleConstraint.Name, lrRoleConstraint.Name, liImageIndex, liImageIndex)
                loNode.Tag = lrRoleConstraint
            Next

            If Me.CheckBoxShowModelDictionary.Checked Then
                For Each lrDictionaryEntry In Me.zrORMModel.ModelDictionary
                    loNode = Me.TreeView1.Nodes("ModelDictionary").Nodes.Add("DictionaryEntry" & lrDictionaryEntry.Symbol, lrDictionaryEntry.Symbol, 0, 0)
                    loNode.Tag = lrDictionaryEntry
                    For Each lrRealisation As pcenumConceptType In lrDictionaryEntry.Realisations
                        loSubNode = loNode.Nodes.Add("Realisation" & lrRealisation.ToString, lrRealisation.ToString, 0, 0)
                        loSubNode.Tag = lrRealisation
                    Next
                Next
            End If

            Me.zrLoadedModel = Me.zrORMModel

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub LoadPGSModelDictionary()

        Try
            Dim loNode As New TreeNode

            loNode = Me.TreeView1.Nodes.Add("Node", "Node", 0, 0)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.pageERD, Nothing)

            If prApplication.WorkingModel Is Nothing Then
                Exit Sub
            End If

            Dim lrEntity As RDS.Table

            prApplication.WorkingModel.RDS.Table.Sort(AddressOf RDS.Table.CompareName)

            Dim loColumnNode As New TreeNode
            For Each lrEntity In prApplication.WorkingModel.RDS.Table
                'loNode = New TreeNode
                If lrEntity.isPGSRelation Then
                    loNode = Me.TreeView1.Nodes("Node").Nodes.Add("Node" & lrEntity.Name, lrEntity.Name, 22, 22)
                Else
                    loNode = Me.TreeView1.Nodes("Node").Nodes.Add("Node" & lrEntity.Name, lrEntity.Name, 21, 21)
                End If

                loNode.Tag = lrEntity

                For Each lrAttribute In lrEntity.Column
                    loColumnNode = loNode.Nodes.Add("Attribute" & lrAttribute.Name, lrAttribute.Name, 14, 14)
                    loColumnNode.Tag = lrAttribute
                Next

            Next

            Me.TreeView1.Nodes(0).Expand()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

        Try
            Dim lrDictionaryEntry As FBM.DictionaryEntry

            Dim lrORMToolboxVerbalisation As frmToolboxORMVerbalisation
            lrORMToolboxVerbalisation = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)

            Me.ToolStripStatusLabelRealisationsCount.Text = "0"
            Me.ToolStripStatusLabelModelElementTypeCount.Text = "0"

            If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

            Select Case e.Node.Tag.GetType.ToString
                Case Is = GetType(FBM.ValueType).ToString
                    lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrLoadedModel, e.Node.Tag.Id, pcenumConceptType.ValueType)
                    lrDictionaryEntry = Me.zrLoadedModel.AddModelDictionaryEntry(lrDictionaryEntry)

                    Me.ToolStripStatusLabelRealisationsCount.Text = lrDictionaryEntry.Realisations.Count
                    Me.ToolStripStatusLabelModelElementTypeCount.Text = lrDictionaryEntry.ModelElementTypeCount

                    '--------------------------------------------------------------
                    'Show the Verbalisation if the Verbalisation Toolbox is open.
                    '--------------------------------------------------------------
                    If IsSomething(lrORMToolboxVerbalisation) Then
                        lrORMToolboxVerbalisation.VerbaliseValueType(e.Node.Tag)
                    End If

                Case Is = GetType(FBM.EntityType).ToString
                    lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrLoadedModel, e.Node.Tag.Id, pcenumConceptType.EntityType)
                    lrDictionaryEntry = Me.zrLoadedModel.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

                    Me.ToolStripStatusLabelRealisationsCount.Text = lrDictionaryEntry.Realisations.Count
                    Me.ToolStripStatusLabelModelElementTypeCount.Text = lrDictionaryEntry.ModelElementTypeCount

                    '--------------------------------------------------------------
                    'Show the Verbalisation if the Verbalisation Toolbox is open.
                    '--------------------------------------------------------------
                    If IsSomething(lrORMToolboxVerbalisation) Then
                        lrORMToolboxVerbalisation.VerbaliseEntityType(e.Node.Tag)
                    End If
                Case Is = GetType(FBM.FactType).ToString
                    '--------------------------------------------------------------
                    'Show the Verbalisation if the Verbalisation Toolbox is open.
                    '--------------------------------------------------------------
                    If IsSomething(lrORMToolboxVerbalisation) Then
                        lrORMToolboxVerbalisation.VerbaliseFactType(e.Node.Tag)
                    End If
                    If prApplication.WorkingPage IsNot Nothing Then

                        Dim lrFactTypeInstance As FBM.FactTypeInstance
                        lrFactTypeInstance = prApplication.WorkingPage.FactTypeInstance.Find(Function(x) x.Id = e.Node.Tag.Id)
                        If lrFactTypeInstance IsNot Nothing Then
                            If Not lrFactTypeInstance.IsSubtypeRelationshipFactType Then
                                Call lrFactTypeInstance.Selected()
                            End If
                        ElseIf e.Node.Tag.IsLinkFactType Then
                            Dim lrFactType As FBM.FactType
                            Dim lrRoleInstance As FBM.RoleInstance

                            lrFactType = e.Node.Tag
                            lrRoleInstance = prApplication.WorkingPage.RoleInstance.Find(Function(x) x.Id = lrFactType.LinkFactTypeRole.Id)

                            If lrRoleInstance IsNot Nothing Then
                                lrRoleInstance.Shape.Brush =
                                              New MindFusion.Drawing.SolidBrush(Color.FromArgb(
                                              [Enum].Parse(GetType(pcenumColourWheel), [Enum].GetName(GetType(pcenumColourWheel), pcenumColourWheel.LightPurple))
                                              ))
                            End If

                        End If
                    End If

                    Dim lrORMReadingEditor As frmToolboxORMReadingEditor
                    lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

                    If IsSomething(lrORMReadingEditor) Then
                        Dim lrFactType As FBM.FactType
                        lrFactType = e.Node.Tag
                        If prApplication.WorkingPage IsNot Nothing Then

                            Dim lrFactTypeInstance As FBM.FactTypeInstance = lrFactType.CloneInstance(prApplication.WorkingPage, False)
                            lrORMReadingEditor.zrPage = prApplication.WorkingPage
                            lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance
                            prApplication.WorkingPage.SelectedObject.Clear()
                            prApplication.WorkingPage.SelectedObject.Add(lrFactTypeInstance)
                            Call lrORMReadingEditor.SetupForm()
                        End If
                    End If

                Case Is = GetType(RDS.Column).ToString
                    If IsSomething(lrORMToolboxVerbalisation) Then
                        lrORMToolboxVerbalisation.VerbaliseColumn(e.Node.Tag)
                    End If
                Case Is = GetType(RDS.Table).ToString
                    If IsSomething(lrORMToolboxVerbalisation) Then
                        lrORMToolboxVerbalisation.VerbaliseTable(e.Node.Tag)
                    End If
                Case Is = GetType(FBM.RoleConstraint).ToString
                    If lrORMToolboxVerbalisation IsNot Nothing Then
                        Dim lrRoleConstraint As FBM.RoleConstraint = e.Node.Tag
                        Select Case lrRoleConstraint.RoleConstraintType
                            Case = pcenumRoleConstraintType.InternalUniquenessConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintInternalUniquenessConstraint(e.Node.Tag)
                            Case Else
                                'not implemented
                        End Select
                    End If
            End Select

            '=====================================================================
            'PropertyGrid
            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If IsSomething(lrPropertyGridForm) Then

                Dim myHiddenMiscAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")

                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myHiddenMiscAttribute})

                Dim lrModelObjectInstance As Object = Nothing
                Select Case e.Node.Tag.GetType
                    Case Is = GetType(FBM.EntityType)
                        lrModelObjectInstance = CType(e.Node.Tag, FBM.EntityType).CloneInstance(New FBM.Page(Me.zrORMModel))
                    Case Is = GetType(FBM.ValueType)
                        lrModelObjectInstance = CType(e.Node.Tag, FBM.ValueType).CloneInstance(New FBM.Page(Me.zrORMModel))
                    Case Is = GetType(FBM.FactType)
                        lrModelObjectInstance = CType(e.Node.Tag, FBM.FactType).CloneInstance(New FBM.Page(Me.zrORMModel))
                    Case Is = GetType(RDS.Table)
                        lrModelObjectInstance = CType(e.Node.Tag, RDS.Table).CloneEntity(New FBM.Page(Me.zrORMModel))
                End Select
                If lrModelObjectInstance IsNot Nothing Then
                    Call lrPropertyGridForm.SetSelectedObject(lrModelObjectInstance)
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub TreeView1_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView1.ItemDrag

        'If Richmond.GetAsyncKeyState(Keys.ControlKey) Then
        '    DoDragDrop(e.Item, DragDropEffects.Move)
        'ElseIf e.Button = Windows.Forms.MouseButtons.Left Then        
        'End If

        If My.Settings.UseClientServer Then
            If Not prApplication.User.ProjectPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0 Then
                Dim lfrmFlashCard As New frmFlashCard
                lfrmFlashCard.ziIntervalMilliseconds = 3500
                lfrmFlashCard.zsText = "Please note that you do not have Alter Permission on this Project."
                Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(Me)
                Exit Sub
            Else
                DoDragDrop(New tShapeNodeDragItem(Me.TreeView1.SelectedNode.Tag), DragDropEffects.Copy)
            End If
        Else
            DoDragDrop(New tShapeNodeDragItem(Me.TreeView1.SelectedNode.Tag), DragDropEffects.Copy)
        End If

    End Sub

    Private Sub TreeView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView1.MouseDown

        Try
            Dim loModelObject As Object = Nothing

            If IsSomething(Me.TreeView1.GetNodeAt(e.Location)) Then
                Me.TreeView1.SelectedNode = Me.TreeView1.GetNodeAt(e.Location)
            End If



            If e.Button = Windows.Forms.MouseButtons.Right Then
                '---------------------------------
                'Select the node under the Mouse
                '---------------------------------
                Me.TreeView1.SelectedNode = Me.TreeView1.GetNodeAt(e.Location)
                If IsSomething(Me.TreeView1.SelectedNode) Then
                    If TypeOf (Me.TreeView1.SelectedNode.Tag) Is tEnterpriseEnterpriseView Then
                        '------------
                        'Do nothing
                        '------------
                        Me.TreeView1.ContextMenuStrip = Nothing
                    Else
                        loModelObject = Me.TreeView1.SelectedNode.Tag
                        Me.ToolStripMenuItemViewOnPage.DropDownItems.Clear()
                        If IsSomething(loModelObject) Then
                            '-----------------------------------------------
                            'Establish the ContextMenu for the SelectedNode
                            '-----------------------------------------------
                            Select Case loModelObject.GetType
                                Case Is = GetType(FBM.EntityType)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
                                    Call Me.LoadPagesForEntityType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(FBM.ValueType)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
                                    Call LoadPagesForValueType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(FBM.FactType)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
                                    Call LoadPagesForFactType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(FBM.RoleConstraint)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
                                    Call LoadPagesForRoleConstraint(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(RDS.Table)
                                    Dim lrTable As RDS.Table = loModelObject
                                    Select Case lrTable.FBMModelElement.GetType
                                        Case Is = GetType(FBM.EntityType)
                                            Call LoadPagesForEntityType(Me.ToolStripMenuItemViewOnPage, lrTable.Name)
                                        Case Is = GetType(FBM.FactType)
                                            Call LoadPagesForFactType(Me.ToolStripMenuItemViewOnPage, lrTable.Name)
                                    End Select
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip1
                                Case Else
                                    Me.TreeView1.ContextMenuStrip = Nothing
                            End Select
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub LoadPagesForEntityType(ByRef aoMenuStripItem As ToolStripMenuItem, ByVal asEntityTypeId As String)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try
            lrModel = prApplication.WorkingModel

            aoMenuStripItem.DropDownItems.Clear()

            Dim lsWorkingPageId As String = Nothing
            If prApplication.WorkingPage IsNot Nothing Then
                lsWorkingPageId = prApplication.WorkingPage.PageId
            End If

            Dim larPage = From Page In lrModel.Page
                          From EntityTypeInstance In Page.EntityTypeInstance
                          Where (EntityTypeInstance.Id = asEntityTypeId)
                          Select Page Distinct
                          Order By Page.Name

            If IsSomething(larPage) Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lr_enterprise_view As tEnterpriseEnterpriseView

                    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               pcenumLanguage.ORMModel,
                                                               Nothing, lrPage.PageId)


                    lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)

                    If IsSomething(lr_enterprise_view) Then
                        '---------------------------------------------------
                        'Add the Page(Name) to the MenuOption.DropDownItems
                        '---------------------------------------------------
                        lr_enterprise_view.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asEntityTypeId)

                        loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                        loToolStripMenuItem.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                        AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
                        aoMenuStripItem.Enabled = True
                    End If

                Next
            Else
                Dim loToolStripMenuItem As ToolStripMenuItem
                loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add("Not yet added to a page")
                loToolStripMenuItem.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub OpenORMDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            '---------------------------------------------------------------------------
            'Find and select the TreeViewNode in the EnterpriseTreeViewer for the Page
            '---------------------------------------------------------------------------
            lr_enterprise_view = item.Tag
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prApplication.WorkingPage = New FBM.Page
            prApplication.WorkingPage = lr_enterprise_view.Tag
            Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub LoadPagesForValueType(ByVal aoMenuStripItem As ToolStripMenuItem, ByVal asValueTypeId As String)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page


        lrModel = prApplication.WorkingModel

        aoMenuStripItem.DropDownItems.Clear()

        Dim lsWorkingPageId As String = Nothing
        If prApplication.WorkingPage IsNot Nothing Then
            lsWorkingPageId = prApplication.WorkingPage.PageId
        End If

        Dim larPage = From Page In lrModel.Page _
                      From ValueTypeInstance In Page.ValueTypeInstance _
                      Where (ValueTypeInstance.Id = asValueTypeId) _
                      Select Page Distinct _
                      Order By Page.Name

        If IsSomething(larPage) Then
            For Each lrPage In larPage

                Dim loToolStripMenuItem As ToolStripMenuItem
                Dim lr_enterprise_view As tEnterpriseEnterpriseView

                lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel, _
                                                           lrPage, _
                                                           lrPage.Model.ModelId, _
                                                           pcenumLanguage.ORMModel, _
                                                           Nothing, lrPage.PageId)

                lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                lr_enterprise_view.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asValueTypeId)
                loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                loToolStripMenuItem.Tag = lr_enterprise_view

                AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
            Next
        Else
            Dim loToolStripMenuItem As ToolStripMenuItem
            loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add("Not yet added to a page")
            loToolStripMenuItem.Enabled = False
        End If

    End Sub

    Sub LoadPagesForFactType(ByVal aoMenuStripItem As ToolStripMenuItem, ByVal asFactTypeId As String)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try
            lrModel = prApplication.WorkingModel

            aoMenuStripItem.DropDownItems.Clear()

            Dim lsWorkingPageId As String = Nothing
            If prApplication.WorkingPage IsNot Nothing Then
                lsWorkingPageId = prApplication.WorkingPage.PageId
            End If

            Dim larPage = From Page In lrModel.Page _
                          From FactTypeInstance In Page.FactTypeInstance _
                          Where (FactTypeInstance.Id = asFactTypeId) _
                          Select Page Distinct _
                          Order By Page.Name

            If IsSomething(larPage) Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lr_enterprise_view As tEnterpriseEnterpriseView

                    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel, _
                                                               lrPage, _
                                                               lrPage.Model.ModelId, _
                                                               pcenumLanguage.ORMModel, _
                                                               Nothing, lrPage.PageId)

                    lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                    lr_enterprise_view.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asFactTypeId)
                    loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                    loToolStripMenuItem.Tag = lr_enterprise_view

                    AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
                Next
            Else
                Dim loToolStripMenuItem As ToolStripMenuItem
                loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add("Not yet added to a page")
                loToolStripMenuItem.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub LoadPagesForRoleConstraint(ByVal aoMenuStripItem As ToolStripMenuItem, ByVal asRoleConstraintId As String)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try

            lrModel = prApplication.WorkingModel

            aoMenuStripItem.DropDownItems.Clear()

            Dim lsWorkingPageId As String = Nothing
            If prApplication.WorkingPage IsNot Nothing Then
                lsWorkingPageId = prApplication.WorkingPage.PageId
            End If

            Dim larPage = From Page In lrModel.Page
                          From RoleConstraintInstance In Page.RoleConstraintInstance
                          Where (RoleConstraintInstance.Id = asRoleConstraintId)
                          Select Page Distinct
                          Order By Page.Name

            If IsSomething(larPage) Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem


                    Dim lr_enterprise_view As tEnterpriseEnterpriseView
                    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               lrPage.Language,
                                                               Nothing, lrPage.PageId)

                    lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)

                    If lr_enterprise_view IsNot Nothing Then
                        lr_enterprise_view.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asRoleConstraintId)
                        loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                        loToolStripMenuItem.Tag = lr_enterprise_view

                        AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
                    End If
                Next
            Else
                Dim loToolStripMenuItem As ToolStripMenuItem
                loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add("Not yet added to a page")
                loToolStripMenuItem.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveFromModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemRemoveFromModel.Click

        Dim lsMessage As String = ""
        Dim lrModelObject As FBM.ModelObject

        Try
            Select Case Me.TreeView1.SelectedNode.Tag.GetType
                Case Is = GetType(RDS.Table)
                    Dim lrTable As RDS.Table = Me.TreeView1.SelectedNode.Tag
                    lrModelObject = lrTable.FBMModelElement
                Case Else
                    lrModelObject = Me.TreeView1.SelectedNode.Tag
            End Select


            lsMessage = "The " & lrModelObject.ConceptType.ToString & ", '" & lrModelObject.Name & "', will be removed from the Model and all Pages."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "This action cannot be undone. Click [OK] to proceed, or [Cancel]."

            If MsgBox(lsMessage, MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation) = MsgBoxResult.Ok Then

                If lrModelObject.RemoveFromModel(False, True, True) Then

                    '----------------------------------------------------------------------------------------------------
                    'NB If Model.StructureChanged event is triggered by the removal of the ModelObject from the Model,
                    '  the whole tree will be refreshed in this form, so no TreeNode will be selected.
                    '  i.e. The node that would otherwise need to be removed, is likely already removed by the refresh.
                    '----------------------------------------------------------------------------------------------------
                    If IsSomething(Me.TreeView1.SelectedNode) Then
                        Me.TreeView1.SelectedNode.Remove()
                    End If

                    Dim lrModel As FBM.Model = lrModelObject.Model
                    Dim larPage As New List(Of FBM.Page)


                    larPage = lrModel.GetPagesContainingModelObject(lrModelObject)

                    'Vm-20180329-The below probably not required.
                    'Dim lrPage As FBM.Page
                    'For Each lrPage In larPage
                    '    lrPage.RemoveModelObject(lrModelObject)
                    'Next
                End If

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub LabelModelName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles LabelModelName.DoubleClick

        MsgBox("ModelId: " & Me.zrORMModel.ModelId)

    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

        Try
            Select Case Me.TreeView1.SelectedNode.Tag.GetType
                Case Is = GetType(FBM.EntityType),
                          GetType(FBM.ValueType),
                          GetType(FBM.FactType)
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = True
                Case Is = GetType(RDS.Table)
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = False
                    Me.ToolStripMenuItemRemoveFromModel.Enabled = True
                    Me.ToolStripMenuItemViewOnPage.Enabled = True
                Case Else
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = False

            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub zrORMModel_MadeDirty(ByVal abGlobalBroadcast As Boolean) Handles zrORMModel.MadeDirty

        If abGlobalBroadcast Then
            If IsSomething(prApplication.WorkingPage) Then
                Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
            Else
                Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel, True)
            End If
        End If

    End Sub

    Private Sub zrORMModel_StructureModified() Handles zrORMModel.StructureModified

        If IsSomething(prApplication.WorkingPage) Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel, True)
        End If

    End Sub

    Private Sub TreeView1_MouseUp(sender As Object, e As MouseEventArgs) Handles TreeView1.MouseUp

        Try
            If prApplication.WorkingPage IsNot Nothing Then
                If prApplication.WorkingPage.Form IsNot Nothing Then
                    If prApplication.WorkingPage.Form.GetType.Name Is frmDiagramORM.GetType.Name Then
                        Call prApplication.WorkingPage.Form.ResetNodeAndLinkColors()
                    End If
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub FindTreeNode(ByVal asSearchString As String)

        For Each lrTreeNode In Me.TreeView1.Nodes
            If Me.FindTreeNodeRecursive(lrTreeNode, asSearchString) Then
                Exit For
            End If
        Next

    End Sub

    Private Function FindTreeNodeRecursive(ByRef arTreeNode As TreeNode, ByVal asSearchString As String) As Boolean

        Dim lrTreeNode As TreeNode

        If arTreeNode.Text = asSearchString Then
            'arTreeNode.EnsureVisible()
            Me.TreeView1.SelectedNode = arTreeNode
            Return True
        End If

        For Each lrTreeNode In arTreeNode.Nodes
            If Me.FindTreeNodeRecursive(lrTreeNode, asSearchString) Then
                Return True
            End If
        Next

        Return False

    End Function

    Private Sub CheckBoxShowCoreModelElements_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShowCoreModelElements.CheckedChanged

        If IsSomething(prApplication.WorkingPage) Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

    End Sub

    Private Sub CheckBoxShowModelDictionary_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShowModelDictionary.CheckedChanged

        If IsSomething(prApplication.WorkingPage) Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        If IsSomething(prApplication.WorkingPage) Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel, True)
        End If

    End Sub

    Private Sub mrApplication_WorkingPageChanged() Handles mrApplication.WorkingPageChanged

        Try
            If Me.mrApplication.WorkingPage.Language <> Me.ziLoadedLanguage Then
                Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ViewInDiagramSpyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemViewInDiagramSpy.Click

        Try
            If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

            Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrLoadedModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

            Dim lrModelObject As FBM.ModelObject

            Try
                lrModelObject = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Id)

                If frmMain.IsDiagramSpyFormLoaded Then
                    prApplication.ActivePages.Find(Function(x) x.Tag.GetType Is GetType(FBM.DiagramSpyPage)).Close()
                End If

                Call frmMain.LoadDiagramSpy(lrDiagramSpyPage, lrModelObject)
            Catch ex As Exception

            End Try


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub MakeNewPageForThisModelElementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemMakeNewPageForThisModelElement.Click

        If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

        Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrLoadedModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

        Dim lrModelObject As Object = Me.TreeView1.SelectedNode.Tag

        Try
            With New WaitCursor

                Dim lsPageName As String = "New Page"
                If lrModelObject Is Nothing Then
                    Exit Sub
                End If

                '===============================================================================
                'Create the Page
                Dim lrPage As FBM.Page = Nothing


                Select Case lrModelObject.GetType
                    Case Is = GetType(FBM.EntityType)

                        lrModelObject = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Id)
                        lsPageName = CType(lrModelObject, FBM.EntityType).Id
                        lsPageName = Me.zrORMModel.CreateUniquePageName(lsPageName, 0)

                        lrPage = New FBM.Page(Me.zrORMModel, Nothing, lsPageName, pcenumLanguage.ORMModel)

                    Case = GetType(RDS.Table)

                        lsPageName = Me.zrORMModel.CreateUniquePageName(lrModelObject.Name, 0)
                        'Clone the Core EntityRelationshipDiagram Page
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
                        lrPage = lrCorePage.Clone(prApplication.WorkingModel)
                        lrPage.Language = pcenumLanguage.EntityRelationshipDiagram
                End Select

                lrPage.Name = lsPageName
                lrPage.Loaded = True
                Me.zrORMModel.Page.Add(lrPage)
                lrPage.Save(True, True)

                Me.zrORMModel.AllowCheckForErrors = True

                Dim lrEnterpriseView As tEnterpriseEnterpriseView = Nothing

                lrEnterpriseView = frmMain.zfrmModelExplorer.AddExistingPageToModel(lrPage, lrPage.Model, lrPage.Model.TreeNode, True)

                MsgBox("Added the new ORM Diagram Page, '" & lrPage.Name & "' to the Model.")

                Select Case lrModelObject.GetType
                    Case Is = GetType(FBM.EntityType)
                        Dim lrEntityType As FBM.EntityType = lrModelObject
                        Call lrPage.DropEntityTypeAtPoint(lrEntityType, New PointF(10, 10), True)
                    Case Is = GetType(RDS.Table)
                        Call lrPage.dropRDSTableAtPoint(lrModelObject, New PointF(10, 10))
                End Select

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub PropertiesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem1.Click

        Try

            Dim lrModelObject As FBM.ModelObject

            Select Case Me.TreeView1.SelectedNode.Tag.GetType
                Case Is = GetType(RDS.Table)
                    lrModelObject = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Name, True)
                Case Is = GetType(FBM.ValueType),
                              GetType(FBM.EntityType),
                              GetType(FBM.FactType),
                              GetType(FBM.RoleConstraint)
                    lrModelObject = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Id, True)
                Case Else
                    Exit Sub
            End Select


            'CodeSafe 
            If lrModelObject Is Nothing Then Exit Sub

            Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrLoadedModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

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

            Call frmMain.LoadToolboxPropertyWindow(frmMain.DockPanel.ActivePane, lrModelObject)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub setLanguage(ByVal aiLanguage As pcenumLanguage)

        Try
            Me.TreeView1.Nodes.Clear()
            RemoveHandler Me.ComboBox1.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged
            Select Case aiLanguage
                Case Is = pcenumLanguage.ORMModel
                    Call Me.LoadORMModelDictionary()
                    Me.ComboBox1.SelectedIndex = 0
                Case Is = pcenumLanguage.EntityRelationshipDiagram
                    Call Me.LoadERDModelDictionary()
                    Me.ComboBox1.SelectedIndex = 1
                Case Is = pcenumLanguage.PropertyGraphSchema
                    Call Me.LoadPGSModelDictionary()
                    Me.ComboBox1.SelectedIndex = 2
            End Select
            AddHandler Me.ComboBox1.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' 20220523-VM-Fixed so caters for ERD and PGS view as well. See main Select Case clause.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ViewInGlossaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewInGlossaryToolStripMenuItem.Click

        Try
            Dim lrModelObject As FBM.ModelObject = Nothing

            If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

            Select Case Me.TreeView1.SelectedNode.Tag.GetType
                Case Is = GetType(RDS.Table)
                    Dim lrTable As RDS.Table = Me.TreeView1.SelectedNode.Tag
                    lrModelObject = lrTable.FBMModelElement
                Case Else
                    lrModelObject = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Id)
            End Select

            Call frmMain.LoadGlossaryForm(lrModelObject)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        Try
            Call Me.TreeView1.Nodes.Clear()
            Select Case Me.ComboBox1.SelectedIndex
                Case Is = 0
                    Call Me.LoadORMModelDictionary()
                Case Is = 1
                    Call Me.LoadERDModelDictionary()
                Case Is = 2
                    Call Me.LoadPGSModelDictionary()
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class