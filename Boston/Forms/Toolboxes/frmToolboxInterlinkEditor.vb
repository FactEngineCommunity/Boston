Imports System.Reflection
Imports System.Linq.Expressions
Imports System.ComponentModel

Public Class frmToolboxInterlinkEditor

    Public WithEvents mrModelElement As FBM.ModelObject
    Private mrBindingList As BindingList(Of Interlink.Interlink)
    Private mrBindingSource As New BindingSource

    Private marInterlink As List(Of Interlink.Interlink)

    Private msOldValue, msNewValue As String 'For Cell editing.

    Private mrSelectedRowDataItem As Interlink.Interlink = Nothing

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmToolboxClassification_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Call Me.SetupForm()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub SetupForm(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try
            Me.mrModelElement = arModelElement

            If arModelElement Is Nothing Then
                Me.DataGridView.DataSource = Nothing
                Me.LabelModelElementName.Text = "<Nothing Selected>"
            Else
                Me.mrBindingList = New BindingList(Of Interlink.Interlink)(Me.mrModelElement.Interlink)
                Me.mrBindingSource.DataSource = Me.mrBindingList
                Me.DataGridView.DataSource = Me.mrBindingSource
                Me.DataGridView.Columns(0).Visible = False
                Me.LabelModelElementName.Text = Me.mrModelElement.Id
            End If

            Me.DataGridView.ReadOnly = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmToolboxDescriptions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub mrModelElement_RemovedFromModel() Handles mrModelElement.RemovedFromModel

        Try
            Me.mrModelElement = Nothing
            Me.LabelModelElementName.Text = "<No Model Element Selected>"
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        Try
            'CodeSafe
            If Me.mrModelElement Is Nothing Then Exit Sub

            ' Create a new object of type KnowledgeGraph.ModelElement
            Dim newModelElement As New Interlink.Interlink(Me.mrModelElement)

            ' Add the new object to the list
            Me.mrModelElement.Interlink.Add(newModelElement)

            '=====================================================================
            'Data Store update
            '20230814-VM-Commented out. Don't want records with Null values.
            'Dim lrConceptModelElement As New Interlink.Interlink(Me.mrModelElement.Model, Me.mrModelElement, "", "")
            'Dim whereClause As Expression(Of Func(Of Interlink.Interlink, Boolean)) = Function(p) p.Concept = newModelElement.Concept And p.Model = newModelElement.Model
            'Dim lrDataStore As New DataStore.Store
            'Call lrDataStore.Upsert(lrConceptModelElement, whereClause)
            '=====================================================================

            Me.mrBindingList = New BindingList(Of Interlink.Interlink)(Me.mrModelElement.Interlink)
            Me.DataGridView.DataSource = Me.mrBindingList
            Me.DataGridView.Columns(0).Visible = False
            ' Refresh the DataGridView to reflect the changes
            Me.DataGridView.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_UserAddedRow(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView.UserAddedRow

        Try
            'CodeSafe
            If Me.mrModelElement Is Nothing Then Exit Sub

            ' Create a new object of type KnowledgeGraph.ModelElement
            Dim lrInterlink As New Interlink.Interlink(Me.mrModelElement)

            ' Add the new object to the list
            Me.mrModelElement.Interlink(Me.mrModelElement.Interlink.Count - 1) = lrInterlink

            '=====================================================================
            'Data Store update
            '20230814-VM-Commented out. Don't want records with Null values.
            'Dim whereClause As Expression(Of Func(Of Interlink.Interlink, Boolean)) = Function(p) p.Concept = lrConceptModelElement.Concept And p.Model = lrConceptModelElement.Model
            'Dim lrDataStore As New DataStore.Store
            'Call lrDataStore.Upsert(lrConceptModelElement, whereClause)
            '=====================================================================

            Me.mrBindingList = New BindingList(Of Interlink.Interlink)(Me.mrModelElement.Interlink)
            Me.mrBindingSource.DataSource = Me.mrBindingList
            Me.DataGridView.DataSource = Me.mrBindingSource
            Me.DataGridView.Columns(0).Visible = False
            Me.DataGridView.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridView.RowsAdded

        Try
            ' Check if the DataGridView is in edit mode
            If DataGridView.IsCurrentCellInEditMode Then
                ' Cancel the edit mode before adding a new row
                DataGridView.EndEdit()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub DataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView.CellBeginEdit
        Try
            '=====Test=================
            Dim popupForm As New frmModelElementSelector
            If popupForm.ShowDialog() = DialogResult.OK Then

                If popupForm.Result Is Nothing Then
                    Exit Sub
                Else
                    Me.DataGridView.Rows(e.RowIndex).Cells(1).Value = popupForm.Result.TargetModelId
                    Me.DataGridView.Rows(e.RowIndex).Cells(3).Value = popupForm.Result.TargetModelElementId

                    Try
                        Me.DataGridView.Rows(e.RowIndex).DataBoundItem.TargetModelId = popupForm.Result.TargetModelId
                        Me.DataGridView.Rows(e.RowIndex).DataBoundItem.TargetModelElementId = popupForm.Result.TargetModelElementId
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try
                End If

            End If

            '======================================================
            ' Check if the edited cell is in the Model column (second column)
            If e.ColumnIndex = 2 Then ' Assuming Model column is at index 1 (0-based index)

                Try
                    ' Get the associated ConceptModelElement object from the BindingList
                    Dim conceptModelElement As Interlink.Interlink = CType(DataGridView.Rows(e.RowIndex).DataBoundItem, Interlink.Interlink)



                    ' Store the old Model value before editing starts
                    'Me.msOldModel = conceptModelElement.Model
                Catch ex As Exception
                    GoTo LoadCombo
                End Try
            End If

LoadCombo:
            Dim cmbcell As New DataGridViewComboBoxCell

            Me.msOldValue = Me.DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

            Dim larComboList As New List(Of Object)
            'Select Case e.ColumnIndex
            '    Case Is = 2
            '        larComboList = Me.marInterlink.GroupBy(Function(item) item.Model).Select(Function(group) group.Key).ToList()
            '    Case Is = 3
            '        Dim lsFilterValue = Me.DataGridView.Rows(e.RowIndex).Cells(2).Value
            '        If lsFilterValue = "" Then
            '            larComboList = Me.marInterlink.GroupBy(Function(item) item.ModelElement).Select(Function(group) group.Key).ToList()
            '        Else
            '            larComboList = Me.marInterlink.Where(Function(x) x.Model = lsFilterValue).GroupBy(Function(item) item.ModelElement).Select(Function(group) group.Key).ToList()
            '        End If
            'End Select

            'If larComboList.Count > 0 Then
            '    cmbcell.Items.AddRange(larComboList.ToArray()) ' Convert list of strings to array and add to ComboBox items
            '    Me.DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex) = cmbcell
            'End If


        Catch ex As Exception
            ' Handle the exception
            ' ...
            e.Cancel = True
            BeginInvoke(Sub()
                            ' Create a new TextBox cell and set its value
                            Dim textBoxCell As New DataGridViewTextBoxCell()
                            textBoxCell.Value = Me.msNewValue

                            ' Replace the ComboBox cell with the TextBox cell
                            Me.DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex) = textBoxCell
                        End Sub)
        End Try
    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView.CellEndEdit

        Try
            Dim lrDataStore As New DataStore.Store
            Dim whereClause As Expression(Of Func(Of Interlink.Interlink, Boolean)) =
                Function(p) p.ModelId = Me.mrModelElement.Model.ModelId And p.ModelElementId = Me.mrModelElement.Id

            Dim lrInterlink = Me.DataGridView.Rows(e.RowIndex).DataBoundItem
            If lrInterlink IsNot Nothing Then
                lrDataStore.Upsert(lrInterlink, whereClause)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView.DataError

        Try
            Call prApplication.ThrowMessage("Check field values", pcenumErrorType.Warning, abThrowtoMSGBox:=True, abUseFlashCard:=True, abSuppressLogging:=True)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView.UserDeletedRow

        Try
            'CodeSafe
            If Me.mrSelectedRowDataItem Is Nothing Then Exit Sub

            Dim lrInterlink As Interlink.Interlink = Me.mrSelectedRowDataItem

            Dim lrDataStore As New DataStore.Store
            Dim loWhereClause As Expression(Of Func(Of Interlink.Interlink, Boolean)) =
                    Function(p) p.ModelId = lrInterlink.ModelId And
                    p.ModelElementId = lrInterlink.ModelElementId And
                    p.TargetModelId = lrInterlink.TargetModelId And
                    p.TargetModelElementId = lrInterlink.TargetModelElementId
            Call lrDataStore.Delete(Of Interlink.Interlink)(loWhereClause)

            Me.mrSelectedRowDataItem = Nothing

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles DataGridView.UserDeletingRow

        Try
            Me.mrSelectedRowDataItem = Me.DataGridView.Rows(e.Row.Index).DataBoundItem
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


End Class