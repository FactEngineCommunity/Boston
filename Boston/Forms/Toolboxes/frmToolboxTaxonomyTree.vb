Imports System.Reflection
Imports Boston.FBM

Public Class frmToolboxTaxonomyTree

    Public WithEvents mrModel As FBM.Model

    Private Sub frmToolboxClientServerBroadcastTester_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try
            Me.LabelModelName.Text = Me.mrModel.Name

            Me.TreeView.Nodes.Add(Me.mrModel.Name, Me.mrModel.Name, 0, 0)
            Call Me.GenerateTreeLayout()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub GenerateTreeLayout(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try
            Try
                Me.TreeView.Nodes(0).Remove()
            Catch
            End Try
            Me.TreeView.Nodes.Clear()
            Me.TreeView.Nodes.Add(Me.mrModel.Name, Me.mrModel.Name, 0, 0)

            Dim larSortedTables = Me.mrModel.RDS.Table.OrderBy(Function(x) x.getSupertypeTables.Count).OrderBy(Function(x) x.Name)
            Dim larAddedTables As New List(Of RDS.Table)

            Dim lrShowTreeNode As cTreeNode = Nothing

            For Each lrTable In larSortedTables

                If lrTable.getSupertypeTables.Count = 0 Then

                    Dim lrTreeNode = New Boston.cTreeNode(lrTable.Name, lrTable.Name, 1, 1)
                    lrTreeNode.Tag = lrTable.FBMModelElement
                    Me.TreeView.Nodes(0).Nodes.Add(lrTreeNode)

                    Call Me.TreeViewTreeNodeGetChildren(lrTreeNode, lrTable, lrShowTreeNode, arModelElement)

                    If arModelElement IsNot Nothing Then
                        If lrTable.FBMModelElement.Id = arModelElement.Id Then
                            lrShowTreeNode = lrTreeNode
                        End If
                    End If

                End If
            Next

            Me.TreeView.ExpandAll()

            'Show the Treenode for a ModelElement passed to the method.
            If arModelElement IsNot Nothing And lrShowTreeNode IsNot Nothing Then
                lrShowTreeNode.EnsureVisible()
                Me.TreeView.SelectedNode = lrShowTreeNode
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TreeViewTreeNodeGetChildren(ByRef arTreeNode As cTreeNode,
                                            ByRef arTable As RDS.Table,
                                            ByRef arShowTreeNode As cTreeNode,
                                            ByRef arModelElement As FBM.ModelObject)

        Try
            For Each lrTable In arTable.getSubtypeTables(True, False)

                Dim lrTreeNode = New cTreeNode(lrTable.Name, lrTable.Name, 1, 1)
                lrTreeNode.Tag = lrTable.FBMModelElement
                arTreeNode.Nodes.Add(lrTreeNode)

                Call Me.TreeViewTreeNodeGetChildren(lrTreeNode, lrTable, arShowTreeNode, arModelElement)

                If arModelElement IsNot Nothing Then
                    If lrTable.FBMModelElement.Id = arModelElement.Id Then
                        arShowTreeNode = lrTreeNode
                    End If
                End If

            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mrModel_ModelElementAdded(ByRef arModelElement As ModelObject) Handles mrModel.ModelElementAdded

        Try
            Call Me.GenerateTreeLayout(arModelElement)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mrModel_SubtypeRelationshipAdded(ByRef arSubtypeRelationship As tSubtypeRelationship) Handles mrModel.SubtypeRelationshipAdded

        Try
            'CodeSafe
            If arSubtypeRelationship.ModelElement Is Nothing Then Exit Sub

            Call Me.GenerateTreeLayout(arSubtypeRelationship.ModelElement)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mrModel_ModelElementRemoved(ByRef arModelElement As ModelObject) Handles mrModel.ModelElementRemoved

        Try
            Call Me.GenerateTreeLayout()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mrModel_SubtypeRelationshipRemoved(ByRef arSubtypeRelationship As tSubtypeRelationship) Handles mrModel.SubtypeRelationshipRemoved

        Try
            Call Me.GenerateTreeLayout()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView.AfterSelect

        Try
            'Verbalisation and PropertyGrid here.

            Dim lrORMToolboxVerbalisation As frmToolboxORMVerbalisation
            lrORMToolboxVerbalisation = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)

            'CodeSafe

#Region "Verbalisation"
            If e.Node.Tag IsNot Nothing And lrORMToolboxVerbalisation IsNot Nothing Then
                Select Case e.Node.Tag.GetType.ToString
                    Case Is = GetType(FBM.ValueType).ToString

                        '--------------------------------------------------------------
                        'Show the Verbalisation if the Verbalisation Toolbox is open.
                        '--------------------------------------------------------------
                        If IsSomething(lrORMToolboxVerbalisation) Then
                            lrORMToolboxVerbalisation.VerbaliseValueType(e.Node.Tag)
                        End If

                    Case Is = GetType(FBM.EntityType).ToString

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
            End If
#End Region

            '-----------------------------------------------------------------------------------------------------------------------
            'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the ORMModel object selected
            '-----------------------------------------------------------------------------------------------------------------------
#Region "Propery Grid"

            Call Me.SetPropertiesGridSelectedObject
#End Region

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SetPropertiesGridSelectedObject()

        Try
            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)


            If IsSomething(lrPropertyGridForm) And IsSomething(Me.TreeView.SelectedNode) Then
                Dim lrModelObject As FBM.ModelObject
                lrModelObject = Me.TreeView.SelectedNode.Tag
                lrPropertyGridForm.PropertyGrid.BrowsableAttributes = Nothing
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing

                Dim lrPage As New FBM.Page(Me.mrModel, Nothing, "DummyPage", pcenumLanguage.ORMModel)

                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
#Region "ValueType"
                        Dim lrValueTypeInstance As FBM.ValueTypeInstance
                        lrValueTypeInstance = lrModelObject
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                        Select Case lrValueTypeInstance.DataType
                            Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                          pcenumORMDataType.NumericDecimal,
                                          pcenumORMDataType.NumericMoney
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            Case Is = pcenumORMDataType.RawDataFixedLength,
                                          pcenumORMDataType.RawDataLargeLength,
                                          pcenumORMDataType.RawDataVariableLength,
                                          pcenumORMDataType.TextFixedLength,
                                          pcenumORMDataType.TextLargeLength,
                                          pcenumORMDataType.TextVariableLength
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                            Case Else
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                        End Select
                        If lrPropertyGridForm.PropertyGrid.SelectedObject IsNot Nothing Then
                            lrPropertyGridForm.PropertyGrid.SelectedObject = New Object
                        End If
                        lrPropertyGridForm.zrSelectedObject = lrValueTypeInstance
                        lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrValueTypeInstance
#End Region
                    Case Is = pcenumConceptType.EntityType
#Region "EntityType"
                        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                        lrEntityTypeInstance = lrModelObject.CloneInstance(lrPage, False)
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DerivationText", True)
                        lrPropertyGridForm.zrSelectedObject = lrEntityTypeInstance
                        If lrEntityTypeInstance.EntityType.HasSimpleReferenceScheme Then
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                            Select Case lrEntityTypeInstance.DataType
                                Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                              pcenumORMDataType.NumericDecimal,
                                              pcenumORMDataType.NumericMoney
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                Case Is = pcenumORMDataType.RawDataFixedLength,
                                              pcenumORMDataType.RawDataLargeLength,
                                              pcenumORMDataType.RawDataVariableLength,
                                              pcenumORMDataType.TextFixedLength,
                                              pcenumORMDataType.TextLargeLength,
                                              pcenumORMDataType.TextVariableLength
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Case Else
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End Select
                        Else
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                        End If
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeInstance
                        lrPropertyGridForm.PropertyGrid.Refresh()
#End Region
                    Case Is = pcenumConceptType.EntityTypeName
                        Dim lrEntityTypeName As FBM.EntityTypeName
                        lrEntityTypeName = lrModelObject
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        If lrEntityTypeName.EntityTypeInstance.EntityType.HasSimpleReferenceScheme Then
                            Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                            Select Case lrEntityTypeName.EntityTypeInstance.DataType
                                Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                              pcenumORMDataType.NumericDecimal,
                                              pcenumORMDataType.NumericMoney
                                    Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                    Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                Case Is = pcenumORMDataType.RawDataFixedLength,
                                              pcenumORMDataType.RawDataLargeLength,
                                              pcenumORMDataType.RawDataVariableLength,
                                              pcenumORMDataType.TextFixedLength,
                                              pcenumORMDataType.TextLargeLength,
                                              pcenumORMDataType.TextVariableLength
                                    Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                    Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Case Else
                                    Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End Select
                        Else
                            Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                            Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            Call lrEntityTypeName.EntityTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                        End If
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeName.EntityTypeInstance
                        lrPropertyGridForm.PropertyGrid.Refresh()
                    Case Is = pcenumConceptType.RoleConstraintRole
                        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                        lrRoleConstraintRoleInstance = lrModelObject
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintRoleInstance.RoleConstraint
                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        lrRoleConstraintInstance = lrModelObject

                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})

                        lrPropertyGridForm.zrSelectedObject = lrModelObject
                        lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.

                        Select Case lrRoleConstraintInstance.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                Dim lrFrequencyConstraintInstance As FBM.FrequencyConstraint
                                lrFrequencyConstraintInstance = lrModelObject
                                Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("Comparitor")
                                Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                                Dim loMiscFilterAttribute5 As Attribute = New System.ComponentModel.CategoryAttribute("Value Constraint")
                                Dim loMiscFilterAttribute6 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4, loMiscFilterAttribute5, loMiscFilterAttribute6})

                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrFrequencyConstraintInstance
                            Case Is = pcenumRoleConstraintType.RoleValueConstraint
                                Dim lrRoleValueConstraintInstance As FBM.RoleValueConstraint
                                lrRoleValueConstraintInstance = lrModelObject
                                lrPropertyGridForm.zrSelectedObject = lrRoleValueConstraintInstance
                                lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleValueConstraintInstance
                            Case Is = pcenumRoleConstraintType.RingConstraint
                                Dim lrRingConstraintInstance As FBM.RingConstraint
                                lrRingConstraintInstance = lrModelObject
                                Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("Comparitor")
                                Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                                Dim loMiscFilterAttribute5 As Attribute = New System.ComponentModel.CategoryAttribute("Value Constraint")
                                Dim loMiscFilterAttribute6 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4, loMiscFilterAttribute5, loMiscFilterAttribute6})

                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRingConstraintInstance
                            Case Else
                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
                        End Select


                    Case Is = pcenumConceptType.FactType
                        Dim lrFactTypeInstance As FBM.FactTypeInstance = lrModelObject.CloneInstance(lrPage, False)

                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "ReferenceMode", lrFactTypeInstance.IsObjectified)
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataType", lrFactTypeInstance.IsObjectified)
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", lrFactTypeInstance.IsObjectified)
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", lrFactTypeInstance.IsObjectified)
                        If lrFactTypeInstance.IsObjectified Then

                            If lrFactTypeInstance.ObjectifyingEntityType.EntityType.HasSimpleReferenceScheme Then
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                                Select Case lrFactTypeInstance.ObjectifyingEntityType.DataType
                                    Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                              pcenumORMDataType.NumericDecimal,
                                              pcenumORMDataType.NumericMoney
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                    Case Is = pcenumORMDataType.RawDataFixedLength,
                                              pcenumORMDataType.RawDataLargeLength,
                                              pcenumORMDataType.RawDataVariableLength,
                                              pcenumORMDataType.TextFixedLength,
                                              pcenumORMDataType.TextLargeLength,
                                              pcenumORMDataType.TextVariableLength
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Case Else
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                End Select
                            Else
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End If
                        End If

                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DerivationText", True)
                        If lrPropertyGridForm.PropertyGrid.SelectedObject IsNot Nothing Then
                            lrPropertyGridForm.PropertyGrid.SelectedObject = New Object
                        End If
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrFactTypeInstance

                    Case pcenumConceptType.Role
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})
                        Dim lrRoleInstance As FBM.RoleInstance = lrModelObject
                        lrPropertyGridForm.zrSelectedObject = lrRoleInstance
                        lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleInstance
                    Case Is = pcenumConceptType.ModelNote
                        Dim lrModelNoteInstance As FBM.ModelNoteInstance = lrModelObject
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("Name")
                        Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("Description (Informal)")
                        Dim loMiscFilterAttribute5 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute,
                                                                                                                                                     loMiscFilterAttribute2,
                                                                                                                                                     loMiscFilterAttribute3,
                                                                                                                                                     loMiscFilterAttribute4,
                                                                                                                                                     loMiscFilterAttribute5})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelNoteInstance
                    Case Else
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelObject
                End Select

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ModelDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call prApplication.setWorkingModel(Me.mrModel)
        Call frmMain.LoadToolboxModelDictionary(True)

    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem.Click

        Try
            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

            Dim lrPropertyGridForm As frmToolboxProperties

            If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                If Me.TreeView.SelectedNode IsNot Nothing Then
                    If Me.TreeView.SelectedNode.Tag IsNot Nothing Then
                        'lrPropertyGridForm.PropertyGrid.SelectedObject = Me.TreeView.SelectedNode.Tag
                    End If
                Else
                    Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
                    Dim myHiddenAttribute As Attribute = New System.ComponentModel.DisplayNameAttribute("Language")
                    Dim myHiddenMiscAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                    ' And you pass it to the PropertyGrid,
                    ' via its BrowsableAttributes property :
                    If My.Settings.SuperuserMode Then
                        lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myHiddenMiscAttribute})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = Me.mrModel
                    Else
                        lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myHiddenAttribute, myHiddenMiscAttribute})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = Me.mrModel
                    End If
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ErrorListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ErrorListToolStripMenuItem.Click

        Call frmMain.loadToolboxErrorListForm(Me.DockPanel.ActivePane)

    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem8.Click

        Try
            Call frmMain.loadToolboxORMReadingEditor(Nothing, Me.DockPanel.ActivePane)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ORMVerbalisationViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ORMVerbalisationViewToolStripMenuItem.Click

        prApplication.WorkingModel = Me.mrModel

        Call frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

    End Sub

    Private Sub RichmondBrainBoxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RichmondBrainBoxToolStripMenuItem.Click

        Try
            prApplication.WorkingModel = Me.mrModel
            prApplication.WorkingPage = Nothing

            frmMain.Cursor = Cursors.WaitCursor
            Call frmMain.loadToolboxRichmondBrainBox(Nothing, Me.DockPanel.ActivePane)
            frmMain.Cursor = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frmToolboxTaxonomyTree_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        Try
            If e.Button = MouseButtons.Right Then
                Me.ContextMenuStrip = Me.ContextMenuStrip_Diagram
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TreeView_MouseDown(sender As Object, e As MouseEventArgs) Handles TreeView.MouseDown

        Try
            Me.TreeView.SelectedNode = Me.TreeView.GetNodeAt(e.Location)

            If Me.TreeView.SelectedNode IsNot Nothing Then Me.TreeView.ForceSelectedNode(Me.TreeView.SelectedNode)
            If e.Button = MouseButtons.Right Then
                If Me.TreeView.SelectedNode IsNot Nothing Then
                    Me.ContextMenuStrip = Me.ContextMenuStrip_TreeViewNode
                Else
                    Me.ContextMenuStrip = Me.ContextMenuStrip_Diagram
                End If

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mrModel_ModelElementModified(ByRef arModelElement As ModelObject) Handles mrModel.ModelElementModified

        Try
            Call Me.GenerateTreeLayout(arModelElement)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ShowInModelDictionaryToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ShowInModelDictionaryToolStripMenuItem1.Click

        Dim lfrmModelDictionary As New frmToolboxModelDictionary

        Try
            If Me.TreeView.SelectedNode Is Nothing Then
                Exit Sub
            End If

            Dim lrModelElement As FBM.ModelObject
            lrModelElement = Me.TreeView.SelectedNode.Tag

            'CodeSafe
            If lrModelElement Is Nothing Then Exit Sub

            If prApplication.RightToolboxForms.FindAll(AddressOf lfrmModelDictionary.EqualsByName).Count = 0 Then
                Call frmMain.LoadToolboxModelDictionary()
            End If

            lfrmModelDictionary = prApplication.RightToolboxForms.Find(AddressOf lfrmModelDictionary.EqualsByName)

            Call lfrmModelDictionary.FindTreeNode(lrModelElement.Id)

            lfrmModelDictionary.Show()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItem10_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem10.Click

        Try
            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

            Call Me.SetPropertiesGridSelectedObject()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ContextMenuStrip_TreeViewNode_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_TreeViewNode.Opening

        Try
            Dim loMenuOption As ToolStripItem

            Dim lrModelElement As Object = Me.TreeView.SelectedNode.Tag


            '--------------------------------------------------------------------
            'ModelErrors - Add menu items for the ModelErrors for the ValueType
            '--------------------------------------------------------------------
            Me.ToolStripMenuItemValueTypeModelErrors.DropDownItems.Clear()

            If lrModelElement IsNot Nothing Then
                Dim lrModelError As FBM.ModelError
                If lrModelElement.ModelError.Count > 0 Then
                    Me.ToolStripMenuItemValueTypeModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
                    For Each lrModelError In lrModelElement.ModelError
                        loMenuOption = Me.ToolStripMenuItemValueTypeModelErrors.DropDownItems.Add(lrModelError.Description)
                        loMenuOption.Image = My.Resources.MenuImages.RainCloudRed16x16
                    Next
                Else
                    Me.ToolStripMenuItemValueTypeModelErrors.Image = My.Resources.MenuImages.Cloud216x16
                    loMenuOption = Me.ToolStripMenuItemValueTypeModelErrors.DropDownItems.Add("There are no Model Errors for this Value Type.")
                    loMenuOption.Image = My.Resources.MenuImages.Cloud216x16
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub
End Class