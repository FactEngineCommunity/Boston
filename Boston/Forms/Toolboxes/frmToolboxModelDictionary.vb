Imports System.Reflection
Imports Boston.FBM

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

    Private Sub frmModelDictionaryLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Call SetupForm()

    End Sub

    Public Sub SetupForm()

        'Setup ComboboxView
        If Me.ComboBoxView.Items.Count = 0 Then
            Me.ComboBoxView.Items.Add(New tComboboxItem(pcenumLanguage.ORMModel, "Object-Role Model - Model Elements"))
            Me.ComboBoxView.Items.Add(New tComboboxItem(pcenumLanguage.EntityRelationshipDiagram, "Entity-Relation - Entities"))
            Me.ComboBoxView.Items.Add(New tComboboxItem(pcenumLanguage.PropertyGraphSchema, "Property Graph - Node Types"))
            Me.ComboBoxView.Items.Add(New tComboboxItem(pcenumLanguage.CMML, "CMML - Common MetaModel Language"))
            Me.ComboBoxView.SelectedIndex = 0
        End If

        If prApplication.WorkingPage IsNot Nothing Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

        If prApplication.WorkingModel IsNot Nothing Then
            Me.zrORMModel = prApplication.WorkingModel
        End If

        If Me.zrORMModel IsNot Nothing Then
            Me.LabelModelName.Text = Me.zrORMModel.Name
        End If

        Me.CheckBoxShowModelDictionary.Visible = My.Settings.SuperuserMode
        Me.CheckBoxShowCoreModelElements.Visible = My.Settings.SuperuserMode

        'ThemeManager.InitializeThemes()
        'ThemeManager.ApplyTheme(Me, piGlobalTheme)

    End Sub

    Private Sub frm_model_dictionary_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        If prApplication.WorkingPage IsNot Nothing Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

    End Sub

    Public Sub LoadToolboxModelDictionary(ByVal ailanguage As pcenumLanguage, Optional ByVal abForceReload As Boolean = False)

        Try
            If Me.zrLoadedModel IsNot Nothing And Not abForceReload Then
                If prApplication.WorkingPage Is Nothing Then Exit Sub
                If (Me.zrLoadedModel Is Me.zrORMModel) And (prApplication.WorkingPage.Language = Me.ziLoadedLanguage) Then
                    Exit Sub
                End If
            End If

            Me.TreeView1.Nodes.Clear()
            Me.Refresh()

            If prApplication.WorkingModel IsNot Nothing Then
                Me.zrORMModel = prApplication.WorkingModel
            End If

            If Me.zrORMModel IsNot Nothing Then
                Me.LabelModelName.Text = Me.zrORMModel.Name
            End If

            Dim liLanguage As pcenumLanguage = Me.ComboBoxView.SelectedItem.ItemData

            If ailanguage <> liLanguage Then
                liLanguage = ailanguage
            End If

            RemoveHandler Me.ComboBoxView.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged
            Select Case liLanguage
                Case Is = pcenumLanguage.ORMModel
                    Call Me.LoadORMModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 0
                Case Is = pcenumLanguage.EntityRelationshipDiagram
                    Call Me.LoadERDModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 1
                Case Is = pcenumLanguage.PropertyGraphSchema
                    Call Me.LoadPGSModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 2
                Case Is = pcenumLanguage.CMML
                    Call Me.LoadCMMLModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 3
            End Select
            AddHandler Me.ComboBoxView.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged

            Me.ziLoadedLanguage = ailanguage

        Catch ex As Exception
            '---------------------------------------------------------------
            'Ignore for now. For some reason this function gets called
            '  when the form is closing and has no access to the TreeView
            '---------------------------------------------------------------
        End Try

    End Sub

    Private Sub LoadERDModelDictionary(Optional ByVal asSearchString As String = Nothing)

        Try
            Dim loNode As New TreeNode

            loNode = Me.TreeView1.Nodes.Add("Entity", "Entity", 0, 0)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.pageERD, Nothing)

            If prApplication.WorkingModel Is Nothing Then
                Exit Sub
            End If

            Dim lrEntity As RDS.Table

            prApplication.WorkingModel.RDS.Table.Sort(AddressOf RDS.Table.CompareName)

            Dim larSearchString() As String = {}
            If asSearchString IsNot Nothing Then
                larSearchString = asSearchString.Split(" ")
            End If

            Dim larTable As List(Of RDS.Table)
            If asSearchString Is Nothing Then
                larTable = prApplication.WorkingModel.RDS.Table
            Else
                'larTable = prApplication.WorkingModel.RDS.Table.FindAll(Function(x) x.Name.LCase.StartsWith(asSearchString.LCase) Or x.Name.LCase.Contains(asSearchString.LCase))
                larTable = prApplication.WorkingModel.RDS.Table.FindAll(Function(x) _
                                                                          larSearchString.Any(Function(s) x.Name.StartsWith(s)) Or
                                                                          x.Name.StartsWith(asSearchString) Or x.Name.Contains(asSearchString))
            End If

            'Display ModelElementCount
            Me.ToolStripStatusLabelModelElementCount.Text = larTable.Count

            Dim loColumnNode As New TreeNode
            For Each lrEntity In larTable
                'loNode = New TreeNode
                loNode = Me.TreeView1.Nodes("Entity").Nodes.Add("Entity" & lrEntity.Name, lrEntity.Name, 33, 33)
                loNode.Tag = lrEntity

                For Each lrColumn In lrEntity.Column
                    Dim liImageInd = Boston.returnIfTrue(lrColumn.isPartOfPrimaryKey, 27, 14)
                    loColumnNode = loNode.Nodes.Add("Attribute" & lrColumn.Name, lrColumn.Name, liImageInd, liImageInd)
                    loColumnNode.Tag = lrColumn
                Next

            Next

            Me.TreeView1.Nodes("Entity").Expand()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub LoadORMModelDictionary(Optional ByRef asSearchString As String = Nothing)

        Dim loNode As TreeNode
        Dim loSubNode As TreeNode

        Try
            Dim lsSearchString = asSearchString

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
            Else
                'CodeSafe
                Me.zrORMModel = prApplication.WorkingModel
            End If

            If Me.zrORMModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept).Count > 0 Then
                loNode = Me.TreeView1.Nodes.Add("General Concepts", "General Concepts", 28, 28)
                loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.modelORMModel, Nothing)
            End If

            Dim lrEntityType As New FBM.EntityType
            Dim larEntityType As New List(Of FBM.EntityType)
            Call prApplication.WorkingModel.EntityType.Sort(AddressOf FBM.EntityType.CompareEntityTypeNames)


            Dim larSearchString() As String = {}
            If lsSearchString IsNot Nothing Then
                larSearchString = lsSearchString.Split(" ")
            End If

            If asSearchString IsNot Nothing Then
                'larEntityType = prApplication.WorkingModel.EntityType.FindAll(Function(x) x.Id.StartsWith(lsSearchString) Or x.Id.LCase.Contains(lsSearchString))
                larEntityType = prApplication.WorkingModel.EntityType.FindAll(Function(x) _
                                                                          larSearchString.Any(Function(s) x.Id.StartsWith(s)) Or
                                                                          x.Id.StartsWith(lsSearchString) Or x.Id.Contains(lsSearchString))
            ElseIf Me.CheckBoxShowCoreModelElements.Checked Then
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
                loNode.ForeColor = Color.SteelBlue
                loNode.NodeFont = New Font(loNode.TreeView.Font, FontStyle.Bold)
            Next

            Dim lrValueType As New FBM.ValueType
            Dim larValueType As New List(Of FBM.ValueType)
            Call prApplication.WorkingModel.ValueType.Sort(AddressOf FBM.ValueType.CompareValueTypeNames)
            If asSearchString IsNot Nothing Then
                'larValueType = prApplication.WorkingModel.ValueType.FindAll(Function(x) x.Id.StartsWith(lsSearchString) Or x.Id.LCase.Contains(lsSearchString))
                larValueType = prApplication.WorkingModel.ValueType.FindAll(Function(x) _
                                                                          larSearchString.Any(Function(s) x.Id.StartsWith(s)) Or
                                                                          x.Id.StartsWith(lsSearchString) Or x.Id.Contains(lsSearchString))
            ElseIf Me.CheckBoxShowCoreModelElements.Checked Then
                larValueType = prApplication.WorkingModel.ValueType
            Else
                larValueType = prApplication.WorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
            End If
            For Each lrValueType In larValueType
                loNode = New TreeNode
                loNode = Me.TreeView1.Nodes("ObjectType").Nodes.Add("ValueType" & lrValueType.Name, lrValueType.Name, 1, 1)
                loNode.Tag = lrValueType
                loNode.ForeColor = Color.SeaGreen
            Next

            Dim larFactType As New List(Of FBM.FactType)
            Dim lrFactType As New FBM.FactType
            Call prApplication.WorkingModel.FactType.Sort(AddressOf FBM.FactType.CompareFactTypeNames)
            If asSearchString IsNot Nothing Then
                'larFactType = prApplication.WorkingModel.FactType.FindAll(Function(x) x.Id.StartsWith(lsSearchString) Or x.Id.LCase.Contains(lsSearchString))
                larFactType = prApplication.WorkingModel.FactType.FindAll(Function(x) _
                                                                          larSearchString.Any(Function(s) x.Id.StartsWith(s)) Or
                                                                          x.Id.StartsWith(lsSearchString) Or x.Id.Contains(lsSearchString))
            ElseIf Me.CheckBoxShowCoreModelElements.Checked Then
                larFactType = prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsLinkFactType = False)
            Else
                larFactType = prApplication.WorkingModel.FactType.FindAll(Function(x) x.IsMDAModelElement = False And x.IsLinkFactType = False)
            End If
            For Each lrFactType In larFactType
                If lrFactType.IsObjectified Then
                    Dim loNode1 = New TreeNode
                    loNode1 = Me.TreeView1.Nodes("ObjectType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 3, 3)
                    loNode1.Tag = lrFactType

                    loNode = loNode1.Nodes.Add("LinkFactTypes" & lrFactType.Name, "Implied Fact Types")
                    loNode.Tag = "LinkFactTypesHolderNode"

                    loNode = loNode1.Nodes.Add("ObjectifyingEntityType" & lrFactType.Name, "Objectifying Entity Type", 26, 26)
                    loNode.Tag = "ObjectifyingEntityTypeHolderNode"


                    loNode = loNode.Nodes.Add("EntityType" & lrFactType.ObjectifyingEntityType.Id, lrFactType.Id, 26, 26)
                    loNode.Tag = lrFactType.ObjectifyingEntityType

                Else
                    loNode = New TreeNode
                    If lrFactType.IsSubtypeRelationshipFactType Then
                        'Subtype Relationship
                        loNode = Me.TreeView1.Nodes("FactType").Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 17, 17)
                    ElseIf lrFactType.Arity = 1 Then
                        'Unary Fact Type
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

                            If lrFactType.getLinkFactTypes.Count > 0 Then
                                Dim loNode1 = loNode.Nodes.Add("LinkFactTypes" & lrFactType.Name, "Implied Fact Types")
                                loNode1.Tag = "LinkFactTypesHolderNode"
                            End If
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
                ElseIf Me.TreeView1.Nodes("FactType").Nodes.Find("LinkFactTypes" & lrObjectifiedFactType.Id, True).Count > 0 Then
                    'FactType is not really Objectified, but none-the-less has Link Fact Types (E.g. Has multi-role Internal Uniqueness Constraint).
                    loNode = Me.TreeView1.Nodes("FactType").Nodes.Find("LinkFactTypes" & lrObjectifiedFactType.Id, True)(0).Nodes.Add("FactType" & lrFactType.Name, lrFactType.Name, 2, 2)
                    loNode.Tag = lrFactType
                End If
            Next

            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim larRoleConstraint As New List(Of FBM.RoleConstraint)
            Dim liImageIndex As Integer = 0
            If asSearchString IsNot Nothing Then
                'larRoleConstraint = prApplication.WorkingModel.RoleConstraint.FindAll(Function(x) x.Id.StartsWith(lsSearchString) Or x.Id.LCase.Contains(lsSearchString))
                larRoleConstraint = prApplication.WorkingModel.RoleConstraint.FindAll(Function(x) _
                                                                                          larSearchString.Any(Function(s) x.Id.StartsWith(s)) Or
                                                                                          x.Id.StartsWith(lsSearchString) Or x.Id.Contains(lsSearchString))
            ElseIf Me.CheckBoxShowCoreModelElements.Checked Then
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

            'General Concepts
            For Each lrDictionaryEntry In prApplication.WorkingModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept).OrderBy(Function(x) x.Symbol)

                loNode = Me.TreeView1.Nodes("General Concepts").Nodes.Add(lrDictionaryEntry.Symbol, lrDictionaryEntry.Symbol, 29, 29)
                loNode.Tag = lrDictionaryEntry
            Next

            If Me.CheckBoxShowModelDictionary.Checked Then
                Dim larDictionaryEntry As List(Of FBM.DictionaryEntry) = Me.zrORMModel.ModelDictionary
                If asSearchString IsNot Nothing Then
                    larDictionaryEntry = Me.zrORMModel.ModelDictionary.FindAll(Function(x) x.Symbol.Contains(lsSearchString))
                End If
                For Each lrDictionaryEntry In larDictionaryEntry
                    loNode = Me.TreeView1.Nodes("ModelDictionary").Nodes.Add("DictionaryEntry" & lrDictionaryEntry.Symbol, lrDictionaryEntry.Symbol, 0, 0)
                    loNode.Tag = lrDictionaryEntry
                    For Each lrRealisation As pcenumConceptType In lrDictionaryEntry.Realisations
                        loSubNode = loNode.Nodes.Add("Realisation" & lrRealisation.ToString, lrRealisation.ToString, 0, 0)
                        loSubNode.Tag = lrRealisation
                    Next
                Next
            End If

            'Display ModelElementCount
            Try
                Me.ToolStripStatusLabelModelElementCount.Text = larValueType.Count + larEntityType.Count + larFactType.Count + larRoleConstraint.Count
            Catch ex As Exception
                'Not a biggie.
            End Try

            Me.zrLoadedModel = Me.zrORMModel

            If asSearchString IsNot Nothing Then
                Me.TreeView1.ExpandAll()
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub LoadCMMLModelDictionary()

        Try
            Dim loNode As New TreeNode
            Dim lrActor As CMML.Actor
            Dim lrProcess As CMML.Process

            If prApplication.WorkingModel Is Nothing Then
                Exit Sub
            End If

#Region "Actors"
            loNode = Me.TreeView1.Nodes.Add("Actor", "Actor", 24, 24)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.pageUMLUseCaseDiagram, Nothing)

            prApplication.WorkingModel.UML.Actor.Sort(AddressOf CMML.Actor.CompareName)

            For Each lrActor In prApplication.WorkingModel.UML.Actor

                loNode = Me.TreeView1.Nodes("Actor").Nodes.Add("Actor" & lrActor.Name, lrActor.Name, 24, 24)
                loNode.Tag = lrActor

                For Each lrProcess In lrActor.Process
                    Dim loProcessNode = loNode.Nodes.Add("Process", lrProcess.Text, 25, 25)
                    loProcessNode.Tag = lrProcess
                Next

            Next
#End Region

#Region "Processes"
            loNode = Me.TreeView1.Nodes.Add("Process", "Process", 25, 25)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.pageUMLUseCaseDiagram, Nothing)

            prApplication.WorkingModel.UML.Process.Sort(AddressOf CMML.Process.CompareText)

            For Each lrProcess In prApplication.WorkingModel.UML.Process

                loNode = Me.TreeView1.Nodes("Process").Nodes.Add("Pocess" & lrProcess.Text, lrProcess.Text, 25, 25)
                loNode.Tag = lrProcess

                'SubProcesses write LINQ
                'For Each lrProcess1 In lrProcess.Process
                '    Dim loSubProcessNode = loNode.Nodes.Add("Process", lrProcess1.Text, 22, 22)
                '    loSubProcessNode.Tag = lrProcess1
                'Next

            Next
#End Region

            Me.TreeView1.Nodes(0).Expand()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub LoadPGSModelDictionary(Optional ByVal asSearchString As String = Nothing)

        Try
            Dim loNode As New TreeNode

            loNode = Me.TreeView1.Nodes.Add("Node", "Node", 0, 0)
            loNode.Tag = New tEnterpriseEnterpriseView(pcenumMenuType.pageERD, Nothing)

            If prApplication.WorkingModel Is Nothing Then
                Exit Sub
            End If

            Dim lrEntity As RDS.Table

            prApplication.WorkingModel.RDS.Table.Sort(AddressOf RDS.Table.CompareName)

            Dim larSearchString() As String = {}
            If asSearchString IsNot Nothing Then
                larSearchString = asSearchString.Split(" ")
            End If

            Dim larTable As List(Of RDS.Table)
            If asSearchString Is Nothing Then
                larTable = prApplication.WorkingModel.RDS.Table
            Else
                'larTable = prApplication.WorkingModel.RDS.Table.FindAll(Function(x) x.Name.LCase.StartsWith(asSearchString.LCase) Or x.Name.LCase.Contains(asSearchString.LCase))
                larTable = prApplication.WorkingModel.RDS.Table.FindAll(Function(x) _
                                                                          larSearchString.Any(Function(s) x.Name.StartsWith(s)) Or
                                                                          x.Name.StartsWith(asSearchString) Or x.Name.Contains(asSearchString))
            End If

            'Display ModelElementCount
            Me.ToolStripStatusLabelModelElementCount.Text = larTable.Count

            Dim loColumnNode As New TreeNode
            For Each lrEntity In larTable
                'loNode = New TreeNode
                If lrEntity.isPGSRelation Then
                    loNode = Me.TreeView1.Nodes("Node").Nodes.Add("Node" & lrEntity.Name, lrEntity.Name, 22, 22)
                Else
                    loNode = Me.TreeView1.Nodes("Node").Nodes.Add("Node" & lrEntity.Name, lrEntity.Name, 21, 21)
                End If

                loNode.Tag = lrEntity

                For Each lrAttribute In lrEntity.Column
                    Dim liImageInd = Boston.returnIfTrue(lrAttribute.isPartOfPrimaryKey, 27, 14)
                    loColumnNode = loNode.Nodes.Add("Attribute" & lrAttribute.Name, lrAttribute.Name, liImageInd, liImageInd)
                    loColumnNode.Tag = lrAttribute
                Next

            Next

            Me.TreeView1.Nodes(0).Expand()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

        Try
            Dim lrDictionaryEntry As FBM.DictionaryEntry

            Dim lrORMToolboxVerbalisation As frmToolboxORMVerbalisation
            lrORMToolboxVerbalisation = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)

            Dim lrModelElementDescriptionsEditor As frmToolboxDescriptions
            lrModelElementDescriptionsEditor = prApplication.GetToolboxForm(frmToolboxDescriptions.Name)

            Me.ToolStripStatusLabelRealisationsCount.Text = "0"
            Me.ToolStripStatusLabelModelElementTypeCount.Text = "0"

            If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

            Dim lrDiagramElement As Object = e.Node.Tag 'Can be Tables/NodeTypes, Attributes/Properties or ORM Model Elements.

            Select Case lrDiagramElement.GetType.ToString
                Case Is = GetType(FBM.DictionaryEntry).ToString

                    Dim lrModelElement As New FBM.ModelObject(lrDiagramElement.Symbol, pcenumConceptType.GeneralConcept)
                    lrModelElement.Model = Me.zrLoadedModel
                    Dim lrModelDictionaryEntry As FBM.DictionaryEntry = lrDiagramElement
                    lrModelElement.ShortDescription = lrModelDictionaryEntry.ShortDescription
                    lrModelElement.LongDescription = lrModelDictionaryEntry.LongDescription

                    '--------------------------------------------
                    'Show the Descriptions for the ModelElement
                    '--------------------------------------------
                    If lrModelElementDescriptionsEditor IsNot Nothing Then
                        Call lrModelElementDescriptionsEditor.setDescriptions(lrModelElement)
                    End If
                Case Is = GetType(FBM.ValueType).ToString
#Region "ValueType"
                    lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrLoadedModel, lrDiagramElement.Id, pcenumConceptType.ValueType)
                    lrDictionaryEntry = Me.zrLoadedModel.AddModelDictionaryEntry(lrDictionaryEntry)

                    Me.ToolStripStatusLabelRealisationsCount.Text = lrDictionaryEntry.Realisations.Count
                    Me.ToolStripStatusLabelModelElementTypeCount.Text = lrDictionaryEntry.ModelElementTypeCount

                    '--------------------------------------------------------------
                    'Show the Verbalisation if the Verbalisation Toolbox is open.
                    '--------------------------------------------------------------
                    If lrORMToolboxVerbalisation IsNot Nothing Then
                        lrORMToolboxVerbalisation.VerbaliseValueType(lrDiagramElement)
                    End If

                    '--------------------------------------------
                    'Show the Descriptions for the ModelElement
                    '--------------------------------------------
                    If lrModelElementDescriptionsEditor IsNot Nothing Then
                        Call lrModelElementDescriptionsEditor.setDescriptions(lrDiagramElement)
                    End If
#End Region
                Case Is = GetType(FBM.EntityType).ToString
#Region "EntityType"
                    lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrLoadedModel, lrDiagramElement.Id, pcenumConceptType.EntityType)
                    lrDictionaryEntry = Me.zrLoadedModel.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

                    Me.ToolStripStatusLabelRealisationsCount.Text = lrDictionaryEntry.Realisations.Count
                    Me.ToolStripStatusLabelModelElementTypeCount.Text = lrDictionaryEntry.ModelElementTypeCount

                    '--------------------------------------------------------------
                    'Show the Verbalisation if the Verbalisation Toolbox is open.
                    '--------------------------------------------------------------
                    If lrORMToolboxVerbalisation IsNot Nothing Then
                        lrORMToolboxVerbalisation.VerbaliseEntityType(lrDiagramElement)
                    End If

                    '--------------------------------------------
                    'Show the Descriptions for the ModelElement
                    '--------------------------------------------
                    If lrModelElementDescriptionsEditor IsNot Nothing Then
                        Call lrModelElementDescriptionsEditor.setDescriptions(lrDiagramElement)
                    End If
#End Region
                Case Is = GetType(FBM.FactType).ToString
#Region "FactType"
                    '--------------------------------------------------------------
                    'Show the Verbalisation if the Verbalisation Toolbox is open.
                    '--------------------------------------------------------------
                    If lrORMToolboxVerbalisation IsNot Nothing Then
                        lrORMToolboxVerbalisation.VerbaliseFactType(lrDiagramElement)
                    End If
                    If prApplication.WorkingPage IsNot Nothing Then

                        Dim lrFactTypeInstance As FBM.FactTypeInstance
                        lrFactTypeInstance = prApplication.WorkingPage.FactTypeInstance.Find(Function(x) x.Id = lrDiagramElement.Id)
                        If lrFactTypeInstance IsNot Nothing Then
                            If Not lrFactTypeInstance.IsSubtypeRelationshipFactType Then
                                Call lrFactTypeInstance.Selected()
                            End If
                        ElseIf lrDiagramElement.IsLinkFactType Then
                            Dim lrFactType As FBM.FactType
                            Dim lrRoleInstance As FBM.RoleInstance

                            lrFactType = lrDiagramElement
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

                    If lrORMReadingEditor IsNot Nothing Then
                        Dim lrFactType As FBM.FactType
                        lrFactType = lrDiagramElement
                        If prApplication.WorkingPage IsNot Nothing Then

                            Dim lrFactTypeInstance As FBM.FactTypeInstance = lrFactType.CloneInstance(prApplication.WorkingPage, False)
                            lrORMReadingEditor.zrPage = prApplication.WorkingPage
                            lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance
                            prApplication.WorkingPage.SelectedObject.Clear()
                            prApplication.WorkingPage.SelectedObject.Add(lrFactTypeInstance)
                            Call lrORMReadingEditor.SetupForm()
                        End If
                    End If

                    '--------------------------------------------
                    'Show the Descriptions for the ModelElement
                    '--------------------------------------------
                    If lrModelElementDescriptionsEditor IsNot Nothing Then
                        Call lrModelElementDescriptionsEditor.setDescriptions(lrDiagramElement)
                    End If

#End Region
                Case Is = GetType(RDS.Column).ToString
#Region "Column"
                    Dim lrColumn As RDS.Column = lrDiagramElement

                    If lrORMToolboxVerbalisation IsNot Nothing Then
                        lrORMToolboxVerbalisation.VerbaliseColumn(lrColumn)
                    End If

                    If lrColumn.FactType IsNot Nothing Then
                        'ORM Reading Editor
                        Dim lrORMReadingEditor As frmToolboxORMReadingEditor
                        lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

                        If lrORMReadingEditor IsNot Nothing Then
                            Dim lrFactType As FBM.FactType
                            lrFactType = lrColumn.FactType
                            If prApplication.WorkingPage IsNot Nothing Then

                                Dim lrFactTypeInstance As FBM.FactTypeInstance = lrFactType.CloneInstance(prApplication.WorkingPage, False)
                                lrORMReadingEditor.zrPage = prApplication.WorkingPage
                                lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance
                                prApplication.WorkingPage.SelectedObject.Clear()
                                prApplication.WorkingPage.SelectedObject.Add(lrFactTypeInstance)
                                Call lrORMReadingEditor.SetupForm()
                            End If
                        End If
                    End If
#End Region
                Case Is = GetType(RDS.Table).ToString
                    If lrORMToolboxVerbalisation IsNot Nothing Then
                        lrORMToolboxVerbalisation.VerbaliseTable(lrDiagramElement)
                    End If
                Case Is = GetType(FBM.RoleConstraint).ToString
                    If lrORMToolboxVerbalisation IsNot Nothing Then
                        Dim lrRoleConstraint As FBM.RoleConstraint = lrDiagramElement
                        Select Case lrRoleConstraint.RoleConstraintType
                            Case = pcenumRoleConstraintType.InternalUniquenessConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintInternalUniquenessConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintUniquenessConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.SubsetConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintSubset(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.EqualityConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintEqualityConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.ExclusionConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintExclusionConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.ExternalFrequencyConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintExternalFrequencyConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintFrequencyConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.InclusiveORConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintInclusiveORConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.RingConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintRingConstraint(lrDiagramElement)
                            Case Is = pcenumRoleConstraintType.ValueComparisonConstraint
                                lrORMToolboxVerbalisation.VerbaliseRoleConstraintValueComparisonConstraint(lrDiagramElement)
                            Case Else
                                'not implemented
                        End Select
                    End If
            End Select

            '=====================================================================
            'PropertyGrid
            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If lrPropertyGridForm IsNot Nothing Then

                Dim myHiddenMiscAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")

                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myHiddenMiscAttribute})

                Dim lrModelObjectInstance As Object = Nothing
                Select Case lrDiagramElement.GetType
                    Case Is = GetType(RDS.Column)
                        Dim lrColumn = CType(lrDiagramElement, RDS.Column)
                        Dim lrAttribute As New ERD.Attribute("DummyId", Nothing, New FBM.Page(Me.zrORMModel))
                        lrAttribute.Name = lrColumn.Name
                        lrAttribute.Column = lrColumn
                        If lrColumn.ActiveRole IsNot Nothing And lrColumn.ActiveRole.JoinsValueType IsNot Nothing Then
                            lrModelObjectInstance = lrAttribute
                        End If
                    Case Is = GetType(FBM.EntityType)
                        lrModelObjectInstance = CType(lrDiagramElement, FBM.EntityType).CloneInstance(New FBM.Page(Me.zrORMModel))
                    Case Is = GetType(FBM.ValueType)
                        lrModelObjectInstance = CType(lrDiagramElement, FBM.ValueType).CloneInstance(New FBM.Page(Me.zrORMModel))
                    Case Is = GetType(FBM.FactType)
                        lrModelObjectInstance = CType(lrDiagramElement, FBM.FactType).CloneInstance(New FBM.Page(Me.zrORMModel))
                    Case Is = GetType(RDS.Table)
                        lrModelObjectInstance = CType(lrDiagramElement, RDS.Table).CloneEntity(New FBM.Page(Me.zrORMModel))
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
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub TreeView1_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView1.ItemDrag

        'If Boston.GetAsyncKeyState(Keys.ControlKey) Then
        '    DoDragDrop(e.Item, DragDropEffects.Move)
        'ElseIf e.Button = Windows.Forms.MouseButtons.Left Then        
        'End If

        Me.TreeView1.AllowDrop = True

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

            If Me.TreeView1.GetNodeAt(e.Location) IsNot Nothing Then
                Me.TreeView1.SelectedNode = Me.TreeView1.GetNodeAt(e.Location)
            End If

            If e.Button = Windows.Forms.MouseButtons.Right Then
                '---------------------------------
                'Select the node under the Mouse
                '---------------------------------
                Me.TreeView1.SelectedNode = Me.TreeView1.GetNodeAt(e.Location)
                If Me.TreeView1.SelectedNode IsNot Nothing Then
                    If TypeOf (Me.TreeView1.SelectedNode.Tag) Is tEnterpriseEnterpriseView Then

                        Dim lrEnterpriseView As tEnterpriseEnterpriseView = Me.TreeView1.SelectedNode.Tag

                        If lrEnterpriseView.MenuType = pcenumMenuType.pageERD And My.Settings.ERDAllowAddEntityTableForm Then
                            Me.TreeView1.ContextMenuStrip = Me.ContextMenuEntity
                        Else
                            '------------
                            'Do nothing
                            '------------
                            Me.TreeView1.ContextMenuStrip = Nothing
                        End If

                    Else
                        loModelObject = Me.TreeView1.SelectedNode.Tag
                        Me.ToolStripMenuItemViewOnPage.DropDownItems.Clear()
                        If loModelObject IsNot Nothing Then
                            '-----------------------------------------------
                            'Establish the ContextMenu for the SelectedNode
                            '-----------------------------------------------
                            Select Case loModelObject.GetType
                                Case Is = GetType(FBM.DictionaryEntry)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStripGeneralConcept
                                Case Is = GetType(FBM.EntityType)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStripMain
                                    Call Me.LoadPagesForEntityType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(FBM.ValueType)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStripMain
                                    Call LoadPagesForValueType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(FBM.FactType)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStripMain
                                    Call LoadPagesForFactType(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(FBM.RoleConstraint)
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStripMain
                                    Call LoadPagesForRoleConstraint(Me.ToolStripMenuItemViewOnPage, loModelObject.Id)
                                Case Is = GetType(RDS.Table)
                                    Dim lrTable As RDS.Table = loModelObject

                                    'CodeSafe
                                    If lrTable.FBMModelElement Is Nothing Then Exit Select

                                    Select Case lrTable.FBMModelElement.GetType
                                        Case Is = GetType(FBM.EntityType)
                                            Call LoadPagesForEntityType(Me.ToolStripMenuItemViewOnPage, lrTable.Name)
                                        Case Is = GetType(FBM.FactType)
                                            Call LoadPagesForFactType(Me.ToolStripMenuItemViewOnPage, lrTable.Name)
                                    End Select
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStripMain
                                Case Is = GetType(RDS.Column)
                                    Dim lrColumn As RDS.Column = loModelObject
                                    Dim lrTable As RDS.Table = lrColumn.Table
                                    Select Case lrTable.FBMModelElement.GetType
                                        Case Is = GetType(FBM.EntityType)
                                            Call LoadPagesForEntityType(Me.ToolStripMenuItemViewOnPage, lrTable.Name)
                                        Case Is = GetType(FBM.FactType)
                                            Call LoadPagesForFactType(Me.ToolStripMenuItemViewOnPage, lrTable.Name)
                                    End Select
                                    Me.TreeView1.ContextMenuStrip = Me.ContextMenuStripMain
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
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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

            Dim larPage As New List(Of FBM.Page)

            Dim larORMPage = From Page In lrModel.Page
                             From EntityTypeInstance In Page.EntityTypeInstance
                             Where (EntityTypeInstance.Id = asEntityTypeId)
                             Select Page Distinct
                             Order By Page.Name

            Dim larERDPage = From Page In lrModel.Page
                             From FactTypeInstance In Page.FactTypeInstance
                             Where FactTypeInstance.Id = pcenumCMMLRelations.CoreElementHasElementType.ToString
                             From Fact In FactTypeInstance.Fact
                             Where Fact("Element").Data = asEntityTypeId
                             Select Page Distinct
                             Order By Page.Name

            larPage.AddRange(larORMPage.ToList)
            larPage.AddRange(larERDPage.ToList)

            If larPage IsNot Nothing Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lrEnterpriseView As tEnterpriseEnterpriseView

                    lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               lrPage.Language,
                                                               Nothing, lrPage.PageId)


                    lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)

                    If lrEnterpriseView IsNot Nothing Then
                        '---------------------------------------------------
                        'Add the Page(Name) to the MenuOption.DropDownItems
                        '---------------------------------------------------
                        lrEnterpriseView.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asEntityTypeId)

                        loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                        loToolStripMenuItem.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                        AddHandler loToolStripMenuItem.Click, AddressOf Me.OpenORMDiagram
                        aoMenuStripItem.Enabled = True
                        Select Case lrPage.Language
                            Case Is = pcenumLanguage.ORMModel
                                loToolStripMenuItem.Image = My.Resources.MenuImages.ORM16x16
                            Case Is = pcenumLanguage.EntityRelationshipDiagram
                                loToolStripMenuItem.Image = My.Resources.MenuImages.ERD16x16
                            Case Is = pcenumLanguage.PropertyGraphSchema
                                loToolStripMenuItem.Image = My.Resources.MenuImages.PGS16x16
                        End Select
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
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Warning, ex.StackTrace, True, False, False)
        End Try

    End Sub

    Sub OpenORMDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            '---------------------------------------------------------------------------
            'Find and select the TreeViewNode in the EnterpriseTreeViewer for the Page
            '---------------------------------------------------------------------------
            lrEnterpriseView = item.Tag
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = lrEnterpriseView.TreeNode
            prApplication.WorkingPage = New FBM.Page
            prApplication.WorkingPage = lrEnterpriseView.Tag
            Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub LoadPagesForValueType(ByVal aoMenuStripItem As ToolStripMenuItem, ByVal asValueTypeId As String)

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
                          From ValueTypeInstance In Page.ValueTypeInstance
                          Where (ValueTypeInstance.Id = asValueTypeId)
                          Select Page Distinct
                          Order By Page.Name

            If larPage IsNot Nothing Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lrEnterpriseView As tEnterpriseEnterpriseView

                    lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               pcenumLanguage.ORMModel,
                                                               Nothing, lrPage.PageId)

                    lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    lrEnterpriseView.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asValueTypeId)
                    loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                    loToolStripMenuItem.Tag = lrEnterpriseView

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, True, False, False)
        End Try

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

            Dim larPage = From Page In lrModel.Page
                          From FactTypeInstance In Page.FactTypeInstance
                          Where (FactTypeInstance.Id = asFactTypeId)
                          Select Page Distinct
                          Order By Page.Name

            If larPage IsNot Nothing Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem
                    Dim lrEnterpriseView As tEnterpriseEnterpriseView

                    lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               pcenumLanguage.ORMModel,
                                                               Nothing, lrPage.PageId)

                    lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    lrEnterpriseView.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asFactTypeId)
                    loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                    loToolStripMenuItem.Tag = lrEnterpriseView

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

            If larPage IsNot Nothing Then
                For Each lrPage In larPage

                    Dim loToolStripMenuItem As ToolStripMenuItem


                    Dim lrEnterpriseView As tEnterpriseEnterpriseView
                    lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               lrPage,
                                                               lrPage.Model.ModelId,
                                                               lrPage.Language,
                                                               Nothing, lrPage.PageId)

                    lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)

                    If lrEnterpriseView IsNot Nothing Then
                        lrEnterpriseView.FocusModelElement = prApplication.WorkingModel.GetModelObjectByName(asRoleConstraintId)
                        loToolStripMenuItem = aoMenuStripItem.DropDownItems.Add(lrPage.Name)
                        loToolStripMenuItem.Tag = lrEnterpriseView

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

            'CodeSafe
            If lrModelObject Is Nothing Then
                If Me.TreeView1.SelectedNode IsNot Nothing Then
                    Me.TreeView1.SelectedNode.Remove()
                End If
                Exit Sub
            End If


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
                    If Me.TreeView1.SelectedNode IsNot Nothing Then
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
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub LabelModelName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles LabelModelName.Click

        MsgBox("ModelId: " & Me.zrORMModel.ModelId)

    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripMain.Opening

        Try
            Me.ViewInGlossaryToolStripMenuItem.Enabled = True
            Me.ToolStripMenuItemMakeNewPageForThisModelElement.Enabled = True

            Select Case Me.TreeView1.SelectedNode.Tag.GetType
                Case Is = GetType(FBM.EntityType),
                          GetType(FBM.ValueType),
                          GetType(FBM.FactType)
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = True
                    Me.ToolStripMenuItemCopyModelElement.Enabled = True
                Case Is = GetType(RDS.Column)
                    Me.ViewInGlossaryToolStripMenuItem.Enabled = False
                    Me.ToolStripMenuItemMakeNewPageForThisModelElement.Enabled = False
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = True
                    Me.ToolStripMenuItemCopyModelElement.Enabled = False
                Case Is = GetType(RDS.Table)
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = False
                    Me.ToolStripMenuItemRemoveFromModel.Enabled = True
                    Me.ToolStripMenuItemViewOnPage.Enabled = True
                    Me.ToolStripMenuItemCopyModelElement.Enabled = False
                    Me.ToolStripMenuItemEditEntity.Enabled = My.Settings.ERDAllowAddEntityTableForm
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = True
                Case Else
                    Me.ToolStripMenuItemViewInDiagramSpy.Enabled = False
                    Me.ToolStripMenuItemCopyModelElement.Enabled = False

            End Select

            If My.Settings.SuperuserMode Then
                Me.ToolStripMenuItemMakeMDAModelElement.Visible = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub zrORMModel_MadeDirty(ByVal abGlobalBroadcast As Boolean) Handles zrORMModel.MadeDirty

        If abGlobalBroadcast Then
            If prApplication.WorkingPage IsNot Nothing Then
                Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
            Else
                Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel, True)
            End If
        End If

    End Sub

    Private Sub zrORMModel_StructureModified() Handles zrORMModel.StructureModified

        If prApplication.WorkingPage IsNot Nothing Then
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
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub FindTreeNode(ByVal asSearchString As String)

        Try
            If Me.zrLoadedModel IsNot Nothing Then
                Dim lrModelElement As FBM.ModelObject = Me.zrLoadedModel.GetModelObjectByName(asSearchString)

                If lrModelElement IsNot Nothing Then

                    Select Case lrModelElement.GetType
                        Case Is = GetType(FBM.FactType)
                            Me.ComboBoxView.SelectedIndex = 0
                            Call Me.LoadORMModelDictionary(asSearchString)
                            Call Me.LoadTree(asSearchString)
                    End Select
                End If
            End If

            Me.SearchTextbox.TextBox.Text = asSearchString.Trim

            For Each lrTreeNode In Me.TreeView1.Nodes
                If Me.FindTreeNodeRecursive(lrTreeNode, asSearchString) Then
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

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

        If prApplication.WorkingPage IsNot Nothing Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

    End Sub

    Private Sub CheckBoxShowModelDictionary_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShowModelDictionary.CheckedChanged

        If prApplication.WorkingPage IsNot Nothing Then
            Call Me.LoadToolboxModelDictionary(prApplication.WorkingPage.Language, True)
        Else
            Call Me.LoadToolboxModelDictionary(pcenumLanguage.ORMModel)
        End If

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        Call Me.RefreshDictionary

    End Sub

    Public Sub RefreshDictionary()

        Dim liCurrentLanguage As pcenumLanguage = pcenumLanguage.ORMModel

        Try
            liCurrentLanguage = Me.ComboBoxView.SelectedItem.ItemData
        Catch ex As Exception
            liCurrentLanguage = pcenumLanguage.ORMModel
        End Try

        Try
            Call Me.LoadToolboxModelDictionary(liCurrentLanguage, True)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ViewInDiagramSpyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemViewInDiagramSpy.Click

        Try
            If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

            Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrLoadedModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

            Dim lrModelObject As FBM.ModelObject = Nothing
            Dim lrTable As RDS.Table = Nothing

            Try
                Select Case Me.TreeView1.SelectedNode.Tag.GetType
                    Case Is = GetType(RDS.Table)
                        lrTable = Me.TreeView1.SelectedNode.Tag
                    Case Is = GetType(RDS.Column)
                        Dim lrColumn As RDS.Column = Me.TreeView1.SelectedNode.Tag
                        lrModelObject = lrColumn.FactType
                    Case Else
                        lrModelObject = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Id)
                End Select

                If frmMain.IsDiagramSpyFormLoaded Then
                    prApplication.ActivePages.Find(Function(x) x.Tag.GetType Is GetType(FBM.DiagramSpyPage)).Close()
                End If

                Select Case Me.TreeView1.SelectedNode.Tag.GetType
                    Case Is = GetType(RDS.Table)
                        Call frmMain.LoadERDDiagramSpy(lrTable)
                    Case Else
                        Call frmMain.LoadDiagramSpy(lrDiagramSpyPage, lrModelObject)
                End Select

            Catch ex As Exception

            End Try


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub setLanguage(ByVal aiLanguage As pcenumLanguage)

        Try
            Me.TreeView1.Nodes.Clear()
            RemoveHandler Me.ComboBoxView.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged
            Select Case aiLanguage
                Case Is = pcenumLanguage.ORMModel
                    Call Me.LoadORMModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 0
                Case Is = pcenumLanguage.EntityRelationshipDiagram
                    Call Me.LoadERDModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 1
                Case Is = pcenumLanguage.PropertyGraphSchema
                    Call Me.LoadPGSModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 2
                Case Is = pcenumLanguage.CMML
                    Call Me.LoadCMMLModelDictionary()
                    Me.ComboBoxView.SelectedIndex = 3
            End Select
            AddHandler Me.ComboBoxView.SelectedIndexChanged, AddressOf Me.ComboBox1_SelectedIndexChanged

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxView.SelectedIndexChanged

        Try
            Dim lrModelElement As Object = Nothing

            If Me.TreeView1.SelectedNode IsNot Nothing Then
                lrModelElement = Me.TreeView1.SelectedNode.Tag
            End If

            Call Me.LoadTree(Nothing, lrModelElement)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub LoadTree(Optional ByVal asSearchString As String = Nothing, Optional ByRef arModelElement As Object = Nothing)

        Try
            If arModelElement IsNot Nothing Then
                Select Case arModelElement.GetType
                    Case Is = GetType(FBM.EntityType),
                              GetType(FBM.ValueType),
                              GetType(FBM.FactType)
                        Me.SearchTextbox.TextBox.Text = arModelElement.Id
                        asSearchString = arModelElement.Id
                    Case Is = GetType(RDS.Table)
                        Me.SearchTextbox.TextBox.Text = arModelElement.Name
                        asSearchString = arModelElement.Name
                    Case Else
                        asSearchString = Nothing
                End Select

            End If

            Call Me.TreeView1.Nodes.Clear()
            Select Case Me.ComboBoxView.SelectedItem.ItemData
                Case Is = pcenumLanguage.ORMModel
                    Call Me.LoadORMModelDictionary(asSearchString)
                Case Is = pcenumLanguage.EntityRelationshipDiagram
                    Call Me.LoadERDModelDictionary(asSearchString)
                Case Is = pcenumLanguage.PropertyGraphSchema
                    Call Me.LoadPGSModelDictionary(asSearchString)
                Case Is = pcenumLanguage.CMML
                    Call Me.LoadCMMLModelDictionary()
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemMakeMDAModelElement_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemMakeMDAModelElement.Click

        Dim lsMessage As String
        Dim lrModelObject As FBM.ModelObject

        Try
            lsMessage = "Are you sure that you want to convert this model element to a MDA Model Element?"
            lsMessage.AppendDoubleLineBreak("This action should only be done by FactEngine staff.")

            If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                Select Case Me.TreeView1.SelectedNode.Tag.GetType
                    Case Is = GetType(RDS.Table)
                        MsgBox("Sorry, this action only applicable to Object-Role Modelling model elements.")
                        Exit Sub
                    Case Else
                        lrModelObject = Me.TreeView1.SelectedNode.Tag

                        Call lrModelObject.SetIsMDAModelElement
                End Select
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SearchTextbox1_InitiateSearch(asSearchString As String) Handles SearchTextbox.InitiateSearch

        Try
            Call Me.LoadTree(asSearchString)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ConvertToEntityTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConvertToEntityTypeToolStripMenuItem.Click

        Try
            Dim lrModelDictionaryEntry As FBM.DictionaryEntry

            Try
                lrModelDictionaryEntry = Me.TreeView1.SelectedNode.Tag

                Call prApplication.WorkingModel.CreateEntityType(lrModelDictionaryEntry.Symbol, True, True, False, False)

                Me.TreeView1.Nodes.Clear()
                Call Me.LoadORMModelDictionary()

            Catch ex As Exception
                'Not a biggie.
            End Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ConvertToValueTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConvertToValueTypeToolStripMenuItem.Click

        Try
            Dim lrModelDictionaryEntry As FBM.DictionaryEntry

            Try
                lrModelDictionaryEntry = Me.TreeView1.SelectedNode.Tag

                Call prApplication.WorkingModel.CreateValueType(lrModelDictionaryEntry.Symbol, True,, True)

                Me.TreeView1.Nodes.Clear()
                Call Me.LoadORMModelDictionary()

            Catch ex As Exception
                'Not a biggie.
            End Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SearchTextbox1_TextBoxCleared() Handles SearchTextbox.TextBoxCleared

        Try
            Call Me.LoadTree()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LineageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineageToolStripMenuItem.Click

        Try
            Dim lrModelObject As FBM.ModelObject = Nothing

            If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

            Select Case Me.TreeView1.SelectedNode.Tag.GetType
                Case Is = GetType(RDS.Table)
                    Dim lrTable As RDS.Table = Me.TreeView1.SelectedNode.Tag
                    lrModelObject = lrTable.FBMModelElement
                Case Is = GetType(RDS.Column)
                    Dim lrColumn As RDS.Column = Me.TreeView1.SelectedNode.Tag
                    lrModelObject = lrColumn.FactType
                Case Else
                    lrModelObject = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Id)
            End Select

            Call frmMain.LoadDataLineageForm(lrModelObject)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemCopyModelElement.Click

        Try
            Dim lrModelElement As FBM.ModelObject = Nothing

            If Me.zrLoadedModel Is Nothing Then Me.zrLoadedModel = Me.zrORMModel

            lrModelElement = Me.zrLoadedModel.GetModelObjectByName(Me.TreeView1.SelectedNode.Tag.Id)

            Dim lrCopyFromPage As New FBM.Page(lrModelElement.Model, "CopyFromPage", "CopyFromPage", pcenumLanguage.ORMModel)

#Region "Add to Page and Copy to Clipboard"
            lrCopyFromPage.SelectedObject.Clear()
            Select Case lrModelElement.GetType

                Case Is = GetType(FBM.ValueType)
                    Dim lrValueTypeInstance = CType(lrModelElement, FBM.ValueType).CloneInstance(lrCopyFromPage, True, True)
                    lrCopyFromPage.SelectedObject.Add(lrValueTypeInstance)

                Case Is = GetType(FBM.EntityType)
                    Dim lrEntityTypeInstance = CType(lrModelElement, FBM.EntityType).CloneInstance(lrCopyFromPage, True, True, False)
                    lrCopyFromPage.SelectedObject.Add(lrEntityTypeInstance)

                Case Is = GetType(FBM.FactType)
                    Dim lrFactTypeInstance = CType(lrModelElement, FBM.FactType).CloneInstance(lrCopyFromPage, True)
                    lrCopyFromPage.SelectedObject.Add(lrFactTypeInstance)

            End Select

            Call frmMain.CopySelectedObjectsToClipboard(lrCopyFromPage)
#End Region

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemAddEntityTable_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAddEntityTable.Click

        Try
            Dim lrTable As RDS.Table = Me.TreeView1.SelectedNode.Tag

            Dim lfrmCRUDAddEditEntityTableForm = frmMain.LoadCRUDERDAddEditEntityTableForm(Me.zrORMModel)

            lfrmCRUDAddEditEntityTableForm.mrRDSTable = lrTable


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub EditEntityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditEntity.Click


        Try
            'CodeSafe
            If TypeOf Me.TreeView1.SelectedNode.Tag IsNot RDS.Table Then Exit Sub

            Dim lrTable As RDS.Table = Me.TreeView1.SelectedNode.Tag

            Dim lfrmCRUDAddEditEntityTableForm = frmMain.LoadCRUDERDAddEditEntityTableForm(Me.zrORMModel, lrTable)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxView_DrawItem(sender As Object, e As DrawItemEventArgs) Handles ComboBoxView.DrawItem
        ' Set the item height
        Me.ComboBoxView.ItemHeight = 20

        ' Determine if the item is selected or hovered
        Dim isHovered As Boolean = (e.State And DrawItemState.HotLight) = DrawItemState.HotLight
        Dim isSelected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected

        ' Set the background color based on the state
        If isSelected Then
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(224, 243, 255)), e.Bounds)
        ElseIf isHovered Then
            ' Use a light pastel blue color when hovering over an item
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(173, 216, 230)), e.Bounds)
        Else
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds)
        End If

        ' Draw the focus rectangle
        e.DrawFocusRectangle()

        ' Check if the item index is valid
        If e.Index >= 0 Then
            Dim liImageIndex As Integer = 1

            ' Select the image index based on the item index
            Select Case e.Index
                Case 0
                    liImageIndex = 31
                Case 1
                    liImageIndex = 12
                Case 2
                    liImageIndex = 30
                Case 3
                    liImageIndex = 32
            End Select

            ' Draw the image from the ImageList
            Dim image As Image = Me.ImageList.Images(liImageIndex)
            ' Calculate image location
            Dim imageLocation As New Point(e.Bounds.Left + 2, e.Bounds.Top + (e.Bounds.Height - image.Height) / 2)
            e.Graphics.DrawImage(image, imageLocation)

            ' Draw the text
            Dim textLocation As New Point(imageLocation.X + image.Width + 5, e.Bounds.Top - 2 + (e.Bounds.Height - TextRenderer.MeasureText(ComboBoxView.Items(e.Index).ToString(), e.Font).Height) / 2)
            e.Graphics.DrawString(ComboBoxView.Items(e.Index).ToString(), e.Font, Brushes.Black, textLocation)
        End If
    End Sub

    Private Sub DynamicDataEntryFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DynamicDataEntryFormToolStripMenuItem.Click

        Try
            Dim lrTable As RDS.Table

            Select Case Me.TreeView1.SelectedNode.Tag.GetType
                Case Is = GetType(RDS.Table)

                    lrTable = Me.TreeView1.SelectedNode.Tag
                    Call prApplication.MainForm.LoadDynamicCRUDForm(lrTable, Nothing)

                Case Is = GetType(FBM.EntityType)

                    Dim lrEntityType As FBM.EntityType = Me.TreeView1.SelectedNode.Tag
                    lrTable = lrEntityType.getCorrespondingRDSTable

                    Call prApplication.MainForm.LoadDynamicCRUDForm(lrTable, Nothing)

                Case Is = GetType(FBM.FactType)

                    Dim lrFactType As FBM.FactType = Me.TreeView1.SelectedNode.Tag

                    If lrFactType.IsObjectified Then
                        lrTable = lrFactType.getCorrespondingRDSTable

                        Call prApplication.MainForm.LoadDynamicCRUDForm(lrTable, Nothing)
                    End If

                Case Is = GetType(PGS.Node)

                    Debugger.Break()

                Case Else
                    Exit Sub
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
        End Try

    End Sub

End Class