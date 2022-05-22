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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonAddConstraint.Click

        Try

            Me.DataGridViewIndexes.Rows.Add()
            Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Cells(0).Value = Trim("UC")
            Dim liIndexType As IndexType = IndexType.Unique
            Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Cells(1).Value = liIndexType

            Dim lrIndex As New RDS.Index(Me.mrTable, "Unique Key")
            lrIndex.IndexQualifier = "UC"
            lrIndex.IsPrimaryKey = False
            Me.DataGridViewIndexes.Rows(Me.DataGridViewIndexes.Rows.Count - 1).Tag = lrIndex

            Me.mrSelectedIndex = lrIndex
            Me.mrApplyTable.Index.Add(lrIndex)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ButtonApply_Click(sender As Object, e As EventArgs) Handles ButtonApply.Click

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

                If lrIndex.Column.Count = 0 Then
                    MsgBox("The Index must contain at least one Column/Property.")
                    Me.ButtonApply.Enabled = False
                    Exit Sub
                End If

                If lrIndex IsNot Nothing Then
                    Dim lrNewIndex = lrIndex.Clone(Me.mrTable)
                    Dim larRole As New List(Of FBM.Role)

                    larRole = New List(Of FBM.Role)
                    Dim lrRoleConstraint As FBM.RoleConstraint
                    Dim liRoleConstraintType As pcenumRoleConstraintType
                    Dim lsRoleConstraintName As String = Nothing

                    Select Case Me.mrTable.FBMModelElement.GetType
                        Case Is = GetType(FBM.EntityType)

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
                                    Call Me.mrTable.FBMModelElement.SetCompoundReferenceSchemeRoleConstraint(lrRoleConstraint)
                                End If
                            Else
                                lrRoleConstraint = larRole(0).FactType.CreateInternalUniquenessConstraint(larRole, lrNewIndex.IsPrimaryKey, True, True, False, Nothing, True, False)
                            End If
                        Case Is = GetType(FBM.FactType)
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

                    End Select
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub DataGridViewIndexes_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DataGridViewIndexes.UserDeletingRow

        Try
            If Me.DataGridViewIndexes.SelectedRows.Count > 1 Then
                e.Cancel = True
                Exit Sub
            End If

            e.Cancel = MessageBox.Show("Do you really want to delete this index?", "", MessageBoxButtons.YesNo) = DialogResult.No

            Dim lrIndex As RDS.Index = e.Row.Tag
            Me.mrApplyTable.Index.Remove(lrIndex)

            Dim lrActualIndex = Me.mrTable.Index.Find(Function(x) x.Name = lrIndex.Name)

            If lrActualIndex Is Nothing Then Exit Sub

            Dim lrRoleConstaint As FBM.RoleConstraint = lrActualIndex.ResponsibleRoleConstraint

            If lrRoleConstaint IsNot Nothing Then
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

                Call Me.mrTable.Model.Model.RemoveRoleConstraint(lrRoleConstaint, True, True, False, True)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

                If lrActualIndex Is Nothing Then Exit Sub

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub DataGridViewIndexes_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewIndexes.CellEndEdit

        Try
            If e.ColumnIndex = 0 Then

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

                Dim lrCandidateIndex = Me.DataGridViewIndexes.Rows(e.RowIndex).Cells(e.ColumnIndex).Tag
                lrCandidateIndex.Name = lsNewIndexName

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