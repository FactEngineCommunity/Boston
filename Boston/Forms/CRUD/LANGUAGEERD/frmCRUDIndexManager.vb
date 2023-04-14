Imports System.ComponentModel
Imports System.Reflection

Public Class frmCRUDIndexManager

    Public mrTable As RDS.Table
    Public mrApplyTable As RDS.Table

    Public mrSelectedIndex As RDS.Index

    Private cManager As CurrencyManager

    ''' <summary>
    ''' Set when the user begins to edit an Index name.
    ''' </summary>
    Private msOriginalIndexNameCellBeginEdit As String

    Private Enum IndexType
        Primary = 0
        Unique = 1
    End Enum

    Private Class Index

        Private _Name As String
        Public Property Name As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
            End Set
        End Property

        Private _Type As IndexType
        Public Property Type As IndexType
            Get
                Return _Type
            End Get
            Set(value As IndexType)
                Me._Type = value
            End Set
        End Property

        Public Sub New(ByVal asName As String, ByVal aiIndexType As IndexType)
            Me.Name = asName
            Me.Type = aiIndexType
        End Sub

    End Class

    Private Class ColumnInclusion

        Private _Include As Boolean
        Public Property Include As Boolean
            Get
                Return _Include
            End Get
            Set(value As Boolean)
                _Include = value
            End Set
        End Property

        Private _ColumnName As String
        Public Property ColumnName As String
            Get
                Return _ColumnName
            End Get
            Set(value As String)
                _ColumnName = value
            End Set
        End Property

        Public Sub New(ByVal abInclude As Boolean, ByVal asColumnName As String)
            Me.Include = abInclude
            Me.ColumnName = asColumnName
        End Sub

        Public Shared Narrowing Operator CType(v As BindingManagerBase) As ColumnInclusion
            Throw New NotImplementedException()
        End Operator
    End Class

    Dim marColumnInclusion As New List(Of ColumnInclusion)

    Private Sub frmCRUDIndexManager_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm()

        Me.LabelPromptTableNodeTypeName.Text = Me.mrTable.Name
        Me.LabelPromotModelName.Text = Me.mrTable.Model.Model.Name

        'Used for applying back to the mrTable, and populating Index and Column DataGridViews.
        Me.mrApplyTable = Me.mrTable.Clone()

        Call Me.DataGridViewIndexes.Columns.Add("Index", "Index")

        Dim loColumn As New DataGridViewComboBoxColumn()
        loColumn.Name = "Type"
        loColumn.DataSource = [Enum].GetValues(GetType(IndexType))
        loColumn.ValueType = GetType(Object) 'GetType(IndexType)

        Call Me.DataGridViewIndexes.Columns.Add(loColumn)

        Call Me.LoadIndexes()

        Call Me.LoadColumns()

    End Sub

    Private Sub LoadColumns()

        Try
            'IMPORTANT: To bind the properties on the Class of the list items must be properties.

            Me.DataGridViewColumns.Refresh()
            Me.DataGridViewColumns.RefreshEdit()

            Dim lbPartOfSelectedIndex As Boolean = False

            For Each lrColumn In Me.mrApplyTable.Column

                lbPartOfSelectedIndex = False

                If Me.DataGridViewIndexes.Rows.Count > 0 Then
                    If CType(Me.DataGridViewIndexes.Rows(0).Tag, RDS.Index).Column.Find(Function(x) x.Id = lrColumn.Id) IsNot Nothing Then
                        lbPartOfSelectedIndex = True
                    End If
                End If

                marColumnInclusion.Add(New ColumnInclusion(lbPartOfSelectedIndex, lrColumn.Name))
            Next

            Me.DataGridViewColumns.DataSource = Me.marColumnInclusion

            Dim liInd = 0
            For Each lrColumn In Me.mrApplyTable.Column
                Me.DataGridViewColumns.Rows(liInd).Tag = lrColumn
                liInd += 1
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadIndexes()

        Try
            Me.DataGridViewIndexes.DataSource = Nothing
            Me.DataGridViewIndexes.Refresh()
            Me.DataGridViewIndexes.RefreshEdit()
            Me.DataGridViewIndexes.Rows.Clear()

            For Each lrIndex In Me.mrApplyTable.Index

                Me.DataGridViewIndexes.Rows.Add()
                Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Cells(0).Value = Trim(lrIndex.Name)

                Dim liIndexType As IndexType = IndexType.Primary
                If Not lrIndex.IsPrimaryKey Then
                    liIndexType = IndexType.Unique
                End If
                Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Cells(1).Value = liIndexType
            Next

            Dim liInd = 0
            For Each lrIndex In Me.mrApplyTable.Index
                Me.DataGridViewIndexes.Rows(liInd).Tag = lrIndex
                liInd += 1
            Next

            'Select the first Row if there is one.
            If Me.DataGridViewIndexes.Rows.Count > 0 Then
                Me.DataGridViewIndexes.Rows(0).Selected = True
                Me.mrSelectedIndex = Me.DataGridViewIndexes.Rows(0).Tag
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonAddConstraint.Click

        Try

            Me.DataGridViewIndexes.Rows.Add()

            Dim lsNewIndexName As String = Me.mrTable.Model.createUniqueIndexName(Me.mrTable.Name & "_UC", 0)

            Dim lrIndex As New RDS.Index(Me.mrTable, lsNewIndexName)
            lrIndex.IndexQualifier = Me.mrTable.generateUniqueQualifier("UC")
            lrIndex.IsPrimaryKey = False

            Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Cells(0).Value = Trim(lsNewIndexName)
            Dim liIndexType As IndexType = IndexType.Unique
            Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Cells(1).Value = liIndexType
            Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Tag = lrIndex

            Me.mrSelectedIndex = lrIndex
            Me.mrApplyTable.Index.Add(lrIndex)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub ButtonApply_Click(sender As Object, e As EventArgs) Handles ButtonApply.Click

        Dim lsMessage As String

        Try
            If Me.DataGridViewIndexes.SelectedRows.Count = 1 Or Me.DataGridViewIndexes.CurrentCell IsNot Nothing Then

                Dim liRowNr As Integer = 0
                If Me.DataGridViewIndexes.SelectedRows.Count = 1 Then
                    liRowNr = Me.DataGridViewIndexes.SelectedRows(0).Index
                Else
                    liRowNr = Me.DataGridViewIndexes.CurrentCell.RowIndex
                    Me.DataGridViewIndexes.Rows(liRowNr).Selected = True
                End If

                Dim lrIndex As RDS.Index = Me.DataGridViewIndexes.Rows(liRowNr).Tag

                '--------------------------------------------------------------------------------
                'Get the Actual Index...if it exists. NB lrActualIndex might be Nothing
                Dim lsNewIndexName = Me.DataGridViewIndexes.Rows(liRowNr).Cells(0).Value
                Dim lrActualIndex = Me.mrTable.Index.Find(Function(x) x.Name = lsNewIndexName)

                Dim lrNewIndex As RDS.Index = Nothing

                If lrActualIndex Is Nothing Then
                    lrNewIndex = lrIndex.Clone(Me.mrTable)

                    Dim larIndex = From Index In Me.mrTable.Index
                                   Where Index.EqualsByColumns(lrNewIndex)
                                   Where Not Index Is lrNewIndex
                                   Select Index

                    If larIndex.Count > 0 Then
                        lrActualIndex = larIndex(0)
                    End If
                End If

#Region "Prechecks"
                If lrIndex.Column.Count = 0 Then
                    MsgBox("The Index must contain at least one Column/Property.")
                    Me.ButtonApply.Enabled = False
                    Exit Sub
                End If
#End Region
                If lrActualIndex Is Nothing Then
                    GoTo ProcessNewIndex
                Else
                    GoTo ProcessExistingIndex
                End If

                Dim larRole As New List(Of FBM.Role)
                Dim lrRoleConstraint As FBM.RoleConstraint
                Dim liRoleConstraintType As pcenumRoleConstraintType
                Dim lsRoleConstraintName As String = Nothing

ProcessExistingIndex:
#Region "Process Existing Index"

                Call lrActualIndex.setName(lsNewIndexName)

                Select Case Me.mrTable.FBMModelElement.GetType
                    Case Is = GetType(FBM.EntityType)
#Region "Existing Index - Entity Type"
                        Dim lrEntityType As FBM.EntityType = Me.mrTable.FBMModelElement
#Region "Prechecks"
                        'Check doesn't clash with an existing Index.
                        Dim larIndex = From Index In Me.mrTable.Index
                                       Where Index.EqualsByColumns(lrIndex)
                                       Where Not Index Is lrIndex
                                       Select Index

                        If larIndex.Count > 0 Then
                            lsMessage = "An Index already exists for the Column/Properties selected."

                            If lrEntityType.HasSimpleReferenceScheme Then
                                lsMessage.AppendDoubleLineBreak("Use the Properties Grid toolbox to remove a Preferred Identifier (Reference Mode) for a model element.")
                            End If
                            MsgBox(lsMessage)
                            Call Me.RevertToActualIndex(lrActualIndex)
                            Exit Sub
                        End If
#End Region



                        larRole = New List(Of FBM.Role)
                        Dim lrFactType As FBM.FactType = Nothing

                        For Each lrColumn In lrIndex.Column
                            lrFactType = lrColumn.FactType
                            If lrColumn.FactType Is Nothing Then
                                lrFactType = lrColumn.Role.FactType
                            End If
                            Dim lrRole = lrFactType.GetOtherRoleOfBinaryFactType(lrColumn.Role.Id)
                            larRole.AddUnique(lrRole)
                        Next

                        lrRoleConstraint = lrActualIndex.getResponsibleRoleConstraintFromORMModel()

                        If lrRoleConstraint IsNot Nothing Then
                            If lrRoleConstraint.IsPreferredIdentifier Then
                                lsMessage = "Use the Properties Grid toolbox to remove a Preferred Identifier (Reference Mode) for a model element."
                                Call Me.RevertToActualIndex(lrActualIndex)
                                Exit Sub
                            Else
                                lrNewIndex = lrIndex.Clone(Me.mrTable)

                                If larRole.Count = 0 Then
                                    '==========================================================================
                                    'Removing a 1:1 RoleConstraint on the far side of a FactType.
                                    Call lrRoleConstraint.RemoveFromModel(True, True, True,,, False)
                                ElseIf larRole.Count = 1 Then
                                    'CodeSafe
                                    If lrActualIndex.Column.Find(Function(x) x.Role.FactType Is lrFactType) IsNot Nothing Then
                                        'Not changing the index at all.
                                        MsgBox("No changes made to the Index.")
                                        GoTo EndProcessing
                                    Else
                                        'Remove existing, add new.
                                        Call lrRoleConstraint.RemoveFromModel(True, True, True,,, False)
                                        lrRoleConstraint = larRole(0).FactType.CreateInternalUniquenessConstraint(larRole, lrNewIndex.IsPrimaryKey, True, True, False, Nothing, True, False)
                                    End If
                                Else
                                    For Each lrColumn In lrActualIndex.Column.ToArray
                                        If lrIndex.Column.Find(Function(x) x.Id = lrColumn.Id) Is Nothing Then
                                            'Remove the Column from the Original/Actual Index.
                                            Dim lrRoleConstraintRole As FBM.RoleConstraintRole = lrRoleConstraint.RoleConstraintRole.Find(Function(x) x.Role Is lrColumn.Role)
                                            Call lrRoleConstraint.RemoveRoleConstraintRole(lrRoleConstraintRole)
                                            Call lrActualIndex.removeColumn(lrColumn)
                                        End If
                                    Next

                                    For Each lrColumn In lrIndex.Column.ToArray
                                        If lrActualIndex.Column.Find(Function(x) x.Id = lrColumn.Id) Is Nothing Then
                                            'Add the Table's corresponding Column to the Original/Actual Index.
                                            Dim lrNewColumn As RDS.Column = Me.mrTable.Column.Find(Function(x) x.Id = lrColumn.Id)
                                            Call lrActualIndex.addColumn(lrColumn)
                                            Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(lrColumn.Role, lrRoleConstraint,,,, True)
                                            Call lrRoleConstraint.AddRoleConstraintRole(lrRoleConstraintRole)
                                        End If
                                    Next
                                End If

                            End If
                        End If
#End Region
                    Case Is = GetType(FBM.FactType)
#Region "Existing Index - Fact Type"
                        '--------------------------------------------------
                        'Check list of Roles are all within same FactType etc
                        Dim lrFactType As FBM.FactType = Me.mrTable.FBMModelElement
                        Dim lrTable As RDS.Table = lrFactType.getCorrespondingRDSTable
                        larRole = New List(Of FBM.Role)

                        Dim lrRole As FBM.Role
                        For Each lrColumn In lrIndex.Column
                            Dim lrColumnFactType As FBM.FactType = lrColumn.FactType

                            If lrColumn.FactType IsNot Nothing Then
                                If lrColumn.FactType.IsLinkFactType Then
                                    'Shouldn't get here because Node Types converted to FactTypes (from EntityType) should move the Role/FactType.
                                    Try
                                        lrColumnFactType = lrColumn.Role.FactType.LinkFactTypeRole.FactType
                                    Catch ex As Exception
                                        lrColumnFactType = lrColumn.Role.FactType
                                    End Try
                                Else
                                    lrColumnFactType = lrColumn.Role.FactType
                                End If
                            ElseIf lrColumn.FactType Is Nothing Then
                                lrColumnFactType = lrColumn.Role.FactType
                            End If
                            If lrColumnFactType.Id = lrFactType.Id And Not lrColumn.Role.FactType.IsLinkFactType Then
                                If lrColumn.FactType Is Nothing Then
                                    lrRole = lrColumn.Role.FactType.LinkFactTypeRole
                                Else
                                    lrRole = lrColumn.Role
                                End If
                            Else
                                lrRole = lrColumnFactType.GetOtherRoleOfBinaryFactType(lrColumn.Role.Id)
                            End If

                            larRole.AddUnique(lrRole)
                        Next

#Region "Prechecking - Existing Index - Fact Type"
                        If lrFactType.ContainsAllRoles(larRole) And larRole.Count < lrFactType.Arity - 1 Then
                            lsMessage = "All of the Columns for the Index," & lrIndex.Name & ", are within the Fact Type for the Table/Node Type, but less than the number of Roles in the Fact Type minus 1."
                            lsMessage.AppendDoubleLineBreak("What this means is that you need to do one of the following from an ORM Diagram view:")
                            lsMessage.AppendLine(vbTab & "1. Add at least one more Column to this pre-existing Index. The Index requires at least " & (lrFactType.Arity - 1).ToString & " Columns, and a maximum of " & lrFactType.Arity.ToString & " Columns; or")
                            lsMessage.AppendLine(vbTab & "2. Remove a Role from the Fact Type.")
                            MsgBox(lsMessage)
                            Me.ButtonApply.Enabled = False
                            Exit Sub
                        End If

                        If Not lrFactType.ContainsAllRoles(larRole, True) Then
                            lsMessage = "Some of the Columns for the Index," & lrIndex.Name & ", are for Roles within the Fact Type for the Table/Node Type, but not all. This does not make sense from an Object-Role Modeling perspective."
                            lsMessage.AppendDoubleLineBreak("What this means is that you need to have one of the following:")
                            lsMessage.AppendLine(vbTab & "1. Include only Columns that are only for Roles within the Fact Type for the Table/Node Type, within the Object-Role Modeling view; or")
                            lsMessage.AppendLine(vbTab & "2. Include only Columns that are for Roles that are outside the Fact Type for the Table/Node Type, within the Object-Role Modeling view.")
                            MsgBox(lsMessage)
                            Me.ButtonApply.Enabled = False
                            Exit Sub
                        End If
#End Region

                        lrRoleConstraint = lrActualIndex.getResponsibleRoleConstraintFromORMModel()

                        If lrRoleConstraint IsNot Nothing Then

                            For Each lrColumn In lrActualIndex.Column.ToArray
                                If lrIndex.Column.Find(Function(x) x.Id = lrColumn.Id) Is Nothing Then
                                    'Remove the Column from the Original/Actual Index.
                                    Dim lrRoleConstraintRole As FBM.RoleConstraintRole = lrRoleConstraint.RoleConstraintRole.Find(Function(x) x.Role Is lrColumn.Role)
                                    Call lrRoleConstraint.RemoveRoleConstraintRole(lrRoleConstraintRole)
                                    Call lrActualIndex.removeColumn(lrColumn)
                                End If
                            Next

                            For Each lrColumn In lrIndex.Column.ToArray
                                If lrActualIndex.Column.Find(Function(x) x.Id = lrColumn.Id) Is Nothing Then
                                    'Add the Table's corresponding Column to the Original/Actual Index.
                                    Dim lrNewColumn As RDS.Column = Me.mrTable.Column.Find(Function(x) x.Id = lrColumn.Id)
                                    Call lrActualIndex.addColumn(lrColumn)
                                    Dim lrRoleConstraintRole As New FBM.RoleConstraintRole(lrColumn.Role, lrRoleConstraint,,,, True)
                                    Call lrRoleConstraint.AddRoleConstraintRole(lrRoleConstraintRole)
                                End If
                            Next
                        ElseIf lrRoleConstraint Is Nothing And lrActualIndex.ResponsibleRoleConstraint Is Nothing Then
                            'Try and Fix the situation.
                            If lrFactType.IsObjectified And lrActualIndex.IsPrimaryKey And lrFactType.getPreferredInternalUniquenessConstraint IsNot Nothing Then
                                lrActualIndex.ResponsibleRoleConstraint = lrFactType.getPreferredInternalUniquenessConstraint
                                For Each lrRole In lrActualIndex.ResponsibleRoleConstraint.Role
                                    Dim larColumn As List(Of RDS.Column) = lrRole.getResponsibleColumns
                                    For Each lrColumn In larColumn
                                        If Not lrActualIndex.Column.Contains(lrColumn) Then
                                            lrActualIndex.addColumn(lrColumn)
                                        End If
                                    Next
                                Next
                            End If
                        End If
#End Region
                End Select

                GoTo EndProcessing
#End Region


ProcessNewIndex:
#Region "Process New Index"
                If lrIndex IsNot Nothing Then
                    lrNewIndex = lrIndex.Clone(Me.mrTable)

                    larRole = New List(Of FBM.Role)

                    Select Case Me.mrTable.FBMModelElement.GetType
                        Case Is = GetType(FBM.EntityType)
#Region "New Index - Entity Type"
#Region "Prechecks"
                            'Check doesn't clash with an existing Index.
                            Dim larIndex = From Index In Me.mrTable.Index
                                           Where Index.EqualsByColumns(lrNewIndex)
                                           Where Not Index Is lrNewIndex
                                           Select Index

                            If larIndex.Count > 0 Then
                                lsMessage = "A unique Index already exists for those Columns/Properties: " & larIndex(0).Name
                                MsgBox(lsMessage)
                                Exit Sub
                            End If
#End Region

                            Dim lrEntityType As FBM.EntityType = Me.mrTable.FBMModelElement

                            For Each lrColumn In lrIndex.Column
                                Dim lrFactType As FBM.FactType = lrColumn.FactType
                                If lrColumn.FactType Is Nothing Then
                                    lrFactType = lrColumn.Role.FactType
                                End If
                                Dim lrRole = lrFactType.GetOtherRoleOfBinaryFactType(lrColumn.Role.Id)
                                larRole.AddUnique(lrRole)
                            Next

                            liRoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint

                            If larRole.Count > 1 And Me.mrTable.FBMModelElement.GetType = GetType(FBM.EntityType) Then

                                liRoleConstraintType = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                lsRoleConstraintName = Me.mrTable.Model.Model.CreateUniqueRoleConstraintName("ExternalUniquenessConstraint", 0)

                                lrRoleConstraint = New FBM.RoleConstraint(Me.mrTable.Model.Model,
                                                                          lsRoleConstraintName,
                                                                          True,
                                                                          liRoleConstraintType,
                                                                          larRole,
                                                                          True)

                                Call Me.mrTable.Model.Model.AddRoleConstraint(lrRoleConstraint, True, True, Nothing, False, Nothing, False)

                                If lrNewIndex.IsPrimaryKey Then
                                    Call lrRoleConstraint.SetIsPreferredIdentifier(True)
                                    Call lrEntityType.SetCompoundReferenceSchemeRoleConstraint(lrRoleConstraint)
                                End If
                            Else
                                lrRoleConstraint = larRole(0).FactType.CreateInternalUniquenessConstraint(larRole, lrNewIndex.IsPrimaryKey, True, True, False, Nothing, True, False)
                                Call Me.mrTable.Model.Model.MakeDirty(True, True)
                            End If
#End Region
                        Case Is = GetType(FBM.FactType)
#Region "New Index - Fact Type"

#Region "Prechecks"
                            'Check doesn't clash with an existing Index.
                            Dim larIndex = From Index In Me.mrTable.Index
                                           Where Index.EqualsByColumns(lrIndex)
                                           Where Not Index Is lrIndex
                                           Select Index

                            If larIndex.Count > 0 Then
                                lsMessage = "A unique Index already exists for those Columns/Properties: " & larIndex(0).Name
                                MsgBox(lsMessage)
                                Exit Sub
                            End If
#End Region

                            '--------------------------------------------------
                            'Check list of Roles are all within same FactType
                            Dim lrFactType As FBM.FactType = Me.mrTable.FBMModelElement
                            Dim lrTable As RDS.Table = lrFactType.getCorrespondingRDSTable

                            Dim lrRole As FBM.Role
                            For Each lrColumn In lrIndex.Column
                                Dim lrColumnFactType As FBM.FactType = lrColumn.FactType
                                If lrColumn.FactType Is Nothing Then
                                    lrColumnFactType = lrColumn.Role.FactType
                                End If
                                If lrColumnFactType.Id = lrFactType.Id Then
                                    lrRole = lrColumn.Role
                                Else
                                    lrRole = lrColumnFactType.GetOtherRoleOfBinaryFactType(lrColumn.Role.Id)
                                End If

                                larRole.AddUnique(lrRole)
                            Next

                            If lrFactType.ContainsAllRoles(larRole) And larRole.Count >= lrFactType.Arity - 1 Then
                                '================================================================================================
                                'All Roles are within the FactType.
                                If lrTable.Index.Find(AddressOf lrNewIndex.EqualsByColumns) IsNot Nothing Then
                                    '----------------------
                                    'Index already exists
                                    MsgBox("An equivalent Index already exists against this Table/Node Type. Modify the Index.")
                                    Exit Sub
                                Else
                                    '----------------------
                                    'Create the new Index
                                    lrRoleConstraint = larRole(0).FactType.CreateInternalUniquenessConstraint(larRole, lrNewIndex.IsPrimaryKey, True, True, False, Nothing, True, False)
                                End If
                            ElseIf Not lrFactType.ContainsAllRoles(larRole) Then

                                liRoleConstraintType = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                lsRoleConstraintName = Me.mrTable.Model.Model.CreateUniqueRoleConstraintName("ExternalUniquenessConstraint", 0)

                                lrRoleConstraint = New FBM.RoleConstraint(Me.mrTable.Model.Model,
                                                                          lsRoleConstraintName,
                                                                          True,
                                                                          liRoleConstraintType,
                                                                          larRole,
                                                                          True)

                                Call Me.mrTable.Model.Model.AddRoleConstraint(lrRoleConstraint, True, True, Nothing, False, Nothing, False)

                                If lrNewIndex.IsPrimaryKey Then
                                    Call lrRoleConstraint.SetIsPreferredIdentifier(True)
                                    Call lrFactType.SetCompoundReferenceSchemeRoleConstraint(lrRoleConstraint)
                                End If
                            End If
#End Region
                    End Select
                End If

EndProcessing:
                Me.ButtonApply.Enabled = False
                Me.ButtonRevert.Enabled = False

            End If
#End Region

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridViewColumns_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewColumns.CellContentClick

        Call Me.DataGridViewColumns.CommitEdit(DataGridViewDataErrorContexts.Commit)

    End Sub

    Private Sub DataGridViewColumns_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewColumns.CellValueChanged

        Try
            If e.ColumnIndex = 0 Then
                'CodeSafe
                If Me.mrSelectedIndex Is Nothing Then Exit Sub

                Select Case Me.DataGridViewColumns.Rows(e.RowIndex).Cells(0).Value
                    Case Is = True
                        Me.mrSelectedIndex.Column.AddUnique(Me.DataGridViewColumns.Rows(e.RowIndex).Tag)
                    Case Is = False
                        Me.mrSelectedIndex.Column.Remove(Me.DataGridViewColumns.Rows(e.RowIndex).Tag)
                End Select

                If mrSelectedIndex.Column.Count = 0 Then
                    MsgBox("The Index must contain at least one Column/Property.")
                    Me.ButtonApply.Enabled = False
                    GoTo EnableRevert
                End If

                Me.ButtonApply.Enabled = True
EnableRevert:
                Me.ButtonRevert.Enabled = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridViewIndexes_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewIndexes.SelectionChanged

        Try
            If Me.DataGridViewIndexes.SelectedRows.Count = 1 Then
                Me.mrSelectedIndex = Me.DataGridViewIndexes.SelectedRows(0).Tag

            ElseIf Me.DataGridViewIndexes.SelectedCells.Count = 1 Then

                Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.SelectedCells(0).RowIndex).Selected = True

                Me.mrSelectedIndex = Me.DataGridViewIndexes.SelectedRows(0).Tag
            ElseIf Me.DataGridViewIndexes.SelectedColumns.Count > 0 Then
                Exit Sub
            End If

            'Update the Column checkboxes (inclusion in Index)
            Dim lrColumn As RDS.Column = Nothing

            If Me.mrSelectedIndex Is Nothing Then Exit Sub

            RemoveHandler Me.DataGridViewColumns.CellValueChanged, AddressOf DataGridViewColumns_CellValueChanged
            For Each lrRow In Me.DataGridViewColumns.Rows

                lrColumn = lrRow.Tag
                lrRow.Cells(0).Value = Me.mrSelectedIndex.Column.Find(Function(x) x.Id = lrColumn.Id) IsNot Nothing
            Next
            AddHandler Me.DataGridViewColumns.CellValueChanged, AddressOf DataGridViewColumns_CellValueChanged

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridViewIndexes_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewIndexes.CellValueChanged

        Try
            Dim loValue As Object = DataGridViewIndexes.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

            Me.mrSelectedIndex = Me.DataGridViewIndexes.Rows(e.RowIndex).Tag

            Select Case e.ColumnIndex
                Case Is = 0
                    'Taken care of in CellEndEdit

                Case Is = 1
                    loValue = CType([Enum].Parse(GetType(IndexType), Trim(loValue)), IndexType)
                    Me.DataGridViewIndexes.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = loValue

                    If Me.DataGridViewIndexes.Rows(e.RowIndex).Tag IsNot Nothing Then
                        If loValue = IndexType.Primary Then
                            Me.DataGridViewIndexes.Rows(e.RowIndex).Tag.IsPrimaryKey = True
                        Else
                            Me.DataGridViewIndexes.Rows(e.RowIndex).Tag.IsPrimaryKey = False
                        End If
                    End If
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridViewIndexes_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DataGridViewIndexes.UserDeletingRow

        Dim lsMessage As String

        Try
            If Me.DataGridViewIndexes.SelectedRows.Count > 1 Then
                e.Cancel = True
                Exit Sub
            End If

            If MessageBox.Show("Do you really want to delete this index?", "", MessageBoxButtons.YesNo) = DialogResult.No Then
                e.Cancel = True
                Exit Sub
            End If

            Dim lrIndex As RDS.Index = e.Row.Tag

            Dim lrActualIndex = Me.mrTable.Index.Find(Function(x) x.Name = lrIndex.Name)

            If lrActualIndex Is Nothing Then Exit Sub

            Dim lrRoleConstaint As FBM.RoleConstraint = lrActualIndex.getResponsibleRoleConstraintFromORMModel

            If lrRoleConstaint IsNot Nothing Then

                Me.mrApplyTable.Index.Remove(lrIndex)

                'Prechecking
                If Me.mrTable.FBMModelElement.GetType = GetType(FBM.EntityType) And
                    lrRoleConstaint.IsPreferredIdentifier And
                    lrRoleConstaint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint And
                    Me.mrTable.Index.FindAll(Function(x) x.IsPrimaryKey).Count = 1 Then
                    'Tryin to delete the ReferenceMode of an EntityType. Don't allow. Direct to PropertiesGrid toolbox.
                    e.Cancel = True

                    lsMessage = "Please use to Properties Grid Toolbox to remove the Reference Mode of the '" & Me.mrTable.Name & "' Node Type."
                    lsMessage.AppendDoubleLineBreak("You cannot delete the Reference Mode/Primary Uniqueness Constraint for a Node Type that is an effective ORM Entity Type with a Reference Mode from this form.")
                    lsMessage.AppendDoubleLineBreak("I.e. The Node Type, " & Me.mrTable.Name & ", is an Entity Type in the Object-Role Model view that has a Reference Mode.")
                    lsMessage.AppendDoubleLineBreak("Right click on the Node Type and select the [Properties] menu item. Set the reference mode to the first item in the list (a blank space).")
                    MsgBox(lsMessage)
                    Exit Sub
                End If

                Select Case Me.mrTable.FBMModelElement.GetType
                    Case Is = GetType(FBM.FactType)

                        If Me.mrTable.FBMModelElement.IsObjectified And lrRoleConstaint.IsPreferredIdentifier Then
                            MsgBox("Can't remove the preferred identifier for an Objectified Fact Type. Modify the model from the Object-Role Model view to remove the Role Constraint associated with this Index/Constraint.")
                            e.Cancel = True
                            Exit Sub
                        Else

                        End If

                    Case Is = GetType(FBM.EntityType)

                        Dim lrEntityType As FBM.EntityType
                        lrEntityType = Me.mrTable.FBMModelElement

                        If lrRoleConstaint.IsPreferredIdentifier Then
                            Call lrRoleConstaint.SetIsPreferredIdentifier(False, False, Nothing)
                        End If

                End Select

                Call lrRoleConstaint.RemoveFromModel(True, True, True, True, True, lrRoleConstaint.IsPreferredIdentifier)
                Call lrRoleConstaint.Model.MakeDirty(True, True)
                'Call Me.mrTable.Model.Model.RemoveRoleConstraint(lrRoleConstaint, True, True, False, True)
            Else
                MsgBox("Boston could not find the responsible Role Constraint for this Index.")
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonRevert_Click(sender As Object, e As EventArgs) Handles ButtonRevert.Click

        Try
            If Me.DataGridViewIndexes.SelectedRows.Count = 1 Or Me.DataGridViewIndexes.CurrentCell IsNot Nothing Then

                Dim liRowNr As Integer = 0
                If Me.DataGridViewIndexes.SelectedRows.Count = 1 Then
                    liRowNr = Me.DataGridViewIndexes.SelectedRows(0).Index
                Else
                    liRowNr = Me.DataGridViewIndexes.CurrentCell.RowIndex
                    Me.DataGridViewIndexes.Rows(liRowNr).Selected = True
                End If

                Dim lrIndex As RDS.Index = Me.DataGridViewIndexes.Rows(liRowNr).Tag
                Dim lrColumn As RDS.Column
                Dim lrActualIndex = Me.mrTable.Index.Find(Function(x) x.Name = lrIndex.Name)

                '20220529-VM-Probably not needed. Remove if need be.
                If lrActualIndex Is Nothing Then
                    MsgBox("Couldn't find the orginal Index against the table, with name: " & lrIndex.Name)
                    Me.ButtonApply.Enabled = False
                    Me.ButtonRevert.Enabled = False
                    Exit Sub
                End If

                For Each lrRow In Me.DataGridViewColumns.Rows
                    lrColumn = lrRow.Tag
                    lrRow.Cells(0).Value = lrActualIndex.Column.Find(Function(x) x.Id = lrColumn.Id) IsNot Nothing
                Next

                Me.ButtonApply.Enabled = False
                Me.ButtonRevert.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RevertToActualIndex(ByRef arActualIndex As RDS.Index)

        Try
            Dim lrColumn As RDS.Column

            For Each lrRow In Me.DataGridViewColumns.Rows
                lrColumn = lrRow.Tag
                lrRow.Cells(0).Value = arActualIndex.Column.Find(Function(x) x.Id = lrColumn.Id) IsNot Nothing
            Next

            Me.ButtonApply.Enabled = False
            Me.ButtonRevert.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridViewIndexes_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridViewIndexes.CellBeginEdit

        Try
            If e.ColumnIndex = 0 Then
                Me.msOriginalIndexNameCellBeginEdit = Me.DataGridViewIndexes.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub DataGridViewIndexes_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewIndexes.CellEndEdit

        Try
            If e.ColumnIndex = 0 Then

                Dim lrCandidateIndex = Me.DataGridViewIndexes.Rows(e.RowIndex).Tag
                Dim lsNewIndexName = Me.DataGridViewIndexes.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                Dim lrActualIndex = Me.mrTable.Index.Find(Function(x) x.Name = lsNewIndexName)

                Dim liInd = 0
                For Each lrRow In Me.DataGridViewIndexes.Rows
                    If LCase(lrRow.Tag.Name) = LCase(lsNewIndexName) Then
                        liInd += 1
                    End If
                Next

                If liInd >= 1 Then
                    Me.DataGridViewIndexes.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Me.msOriginalIndexNameCellBeginEdit
                    Exit Sub
                End If

                lrCandidateIndex.Name = lsNewIndexName

                Me.ButtonApply.Enabled = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class