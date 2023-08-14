Imports System.Reflection
Imports System.Linq.Expressions
Imports System.ComponentModel

Public Class frmToolboxConceptClassification

    Public WithEvents mrModelElement As FBM.ModelObject
    Private mrBindingList As BindingList(Of KnowledgeGraph.ConceptClassificationValue)
    Private msOldClassificationType As String = ""

    Dim mrConceptClassificationReferenceTable As New ReferenceTable(pcenumReferenceTable.ConceptClassification, "ConceptClassification")
    Private marContextClassification As List(Of Object)

    Private msOldValue, msNewValue As String 'For Cell editing.

    Private mrSelectedRowDataItem As KnowledgeGraph.ConceptClassificationValue = Nothing

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmToolboxClassification_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim loSetting As Object = New System.Dynamic.ExpandoObject 'Dummy
        Me.marContextClassification = TableReferenceFieldValue.GetReferenceFieldValueTuples(pcenumReferenceTable.ConceptClassification, loSetting, Me.mrConceptClassificationReferenceTable)

    End Sub

    Public Sub SetupForm(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try
            Me.mrModelElement = arModelElement

            If arModelElement Is Nothing Then
                Me.DataGridView.DataSource = Nothing
                Me.LabelModelElementName.Text = "<Nothing Selected>"
            Else
                Me.mrBindingList = New BindingList(Of KnowledgeGraph.ConceptClassificationValue)(Me.mrModelElement.ClassificationValue)
                Me.DataGridView.DataSource = Me.mrBindingList
                Me.DataGridView.Columns(0).Visible = False
                Me.LabelModelElementName.Text = Me.mrModelElement.Id
            End If

            Me.DataGridView.ReadOnly = False

            'CodeSafe
            Dim larClassificationType = Me.marContextClassification.GroupBy(Function(item) item.ClassificationType).Select(Function(group) group.Key).ToList()
            If Me.DataGridView.Rows.Count >= larClassificationType.Count Then
                'Can't add new Classifications
                'CodeSafe
                Me.DataGridView.AllowUserToAddRows = False
            Else
                Me.DataGridView.AllowUserToAddRows = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        Try
            'CodeSafe
            If Me.mrModelElement Is Nothing Then Exit Sub

            'CodeSafe
            Dim larClassificationType = Me.marContextClassification.GroupBy(Function(item) item.ClassificationType).Select(Function(group) group.Key).ToList()
            If Me.DataGridView.Rows.Count >= larClassificationType.Count Then
                'Can't add new Classifications
                'CodeSafe
                Me.DataGridView.AllowUserToAddRows = False
                Exit Sub
            End If

            ' Create a new object of type KnowledgeGraph.ClassificationValue
            Dim newClassificationValue As New KnowledgeGraph.ConceptClassificationValue(Me.mrModelElement.Model, Me.mrModelElement, "", "")

            ' Add the new object to the list
            Me.mrModelElement.ClassificationValue.Add(newClassificationValue)

            '=====================================================================
            'Data Store update
            '20230814-VM-Commented out. Don't want records with Null values.
            'Dim lrConceptClassificationValue As New KnowledgeGraph.ConceptClassificationValue(Me.mrModelElement.Model, Me.mrModelElement, "", "")
            'Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) = Function(p) p.Concept = newClassificationValue.Concept And p.ClassificationType = newClassificationValue.ClassificationType
            'Dim lrDataStore As New DataStore.Store
            'Call lrDataStore.Upsert(lrConceptClassificationValue, whereClause)
            '=====================================================================

            Me.mrBindingList = New BindingList(Of KnowledgeGraph.ConceptClassificationValue)(Me.mrModelElement.ClassificationValue)
            Me.DataGridView.DataSource = Me.mrBindingList
            Me.DataGridView.Columns(0).Visible = False
            ' Refresh the DataGridView to reflect the changes
            Me.DataGridView.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_UserAddedRow(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView.UserAddedRow

        Try
            'CodeSafe
            If Me.mrModelElement Is Nothing Then Exit Sub

            ' Create a new object of type KnowledgeGraph.ClassificationValue
            Dim lrConceptClassificationValue As New KnowledgeGraph.ConceptClassificationValue(Me.mrModelElement.Model, Me.mrModelElement, "", "")

            ' Set the properties of the new object based on the DataGridView input
            lrConceptClassificationValue.Model = Me.mrModelElement.Model

            ' Add the new object to the list
            Me.mrModelElement.ClassificationValue(Me.mrModelElement.ClassificationValue.Count - 1) = lrConceptClassificationValue

            '=====================================================================
            'Data Store update
            '20230814-VM-Commented out. Don't want records with Null values.
            'Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) = Function(p) p.Concept = lrConceptClassificationValue.Concept And p.ClassificationType = lrConceptClassificationValue.ClassificationType
            'Dim lrDataStore As New DataStore.Store
            'Call lrDataStore.Upsert(lrConceptClassificationValue, whereClause)
            '=====================================================================

            Me.mrBindingList = New BindingList(Of KnowledgeGraph.ConceptClassificationValue)(Me.mrModelElement.ClassificationValue)
            Me.DataGridView.DataSource = Me.mrBindingList
            Me.DataGridView.Columns(0).Visible = False
            Me.DataGridView.Refresh()

            Dim larClassificationType = Me.marContextClassification.GroupBy(Function(item) item.ClassificationType).Select(Function(group) group.Key).ToList()
            If Me.DataGridView.Rows.Count = larClassificationType.Count Then
                'Can't add new Classifications
                'CodeSafe
                Me.DataGridView.AllowUserToAddRows = False
                Exit Sub
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub DataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView.CellBeginEdit
        Try
            ' Check if the edited cell is in the ClassificationType column (second column)
            If e.ColumnIndex = 2 Then ' Assuming ClassificationType column is at index 1 (0-based index)

                Try
                    ' Get the associated ConceptClassificationValue object from the BindingList
                    Dim conceptClassificationValue As KnowledgeGraph.ConceptClassificationValue = CType(DataGridView.Rows(e.RowIndex).DataBoundItem, KnowledgeGraph.ConceptClassificationValue)



                    ' Store the old ClassificationType value before editing starts
                    Me.msOldClassificationType = conceptClassificationValue.ClassificationType
                Catch ex As Exception
                    GoTo LoadCombo
                End Try
            End If

LoadCombo:
            Dim cmbcell As New DataGridViewComboBoxCell

            Me.msOldValue = Me.DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value

            Dim larComboList As New List(Of Object)
            Select Case e.ColumnIndex
                Case Is = 2
                    larComboList = Me.marContextClassification.GroupBy(Function(item) item.ClassificationType).Select(Function(group) group.Key).ToList()
                Case Is = 3
                    Dim lsFilterValue = Me.DataGridView.Rows(e.RowIndex).Cells(2).Value
                    If lsFilterValue = "" Then
                        larComboList = Me.marContextClassification.GroupBy(Function(item) item.ClassificationValue).Select(Function(group) group.Key).ToList()
                    Else
                        larComboList = Me.marContextClassification.Where(Function(x) x.ClassificationType = lsFilterValue).GroupBy(Function(item) item.ClassificationValue).Select(Function(group) group.Key).ToList()
                    End If
            End Select

            If larComboList.Count > 0 Then
                cmbcell.Items.AddRange(larComboList.ToArray()) ' Convert list of strings to array and add to ComboBox items
                Me.DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex) = cmbcell
            End If


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
            ' Check if the edited cell is in the ClassificationType column (second column)
            If e.ColumnIndex = 2 Then ' Assuming ClassificationType column is at index 1 (0-based index)
#Region "ClassificationType"
                Dim lsOldConceptClassificationType = Me.msOldValue
                Dim lsUpdatedClassificationType As String = DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()

                Dim larClassificationType As List(Of String) = Me.mrModelElement.ClassificationValue.Select(Function(x) x.ClassificationType).ToList
                If larClassificationType.Count > 0 Then
                    larClassificationType.RemoveAt(larClassificationType.Count - 1)
                End If

                If larClassificationType.Contains(lsUpdatedClassificationType) Then
                    DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = lsOldConceptClassificationType
                    Exit Sub
                Else
                    If lsOldConceptClassificationType <> "" Then
                        DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = lsOldConceptClassificationType
                        Exit Sub
                    End If
                End If

                '20230814-VM-Don't want to ever update the type
                '' Get the updated ClassificationType value from the cell
                'Dim lsUpdatedClassificationType As String = DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()

                '' Get the associated ConceptClassificationValue object from the BindingList
                'Dim lrConceptClassification As KnowledgeGraph.ConceptClassificationValue = CType(DataGridView.Rows(e.RowIndex).DataBoundItem, KnowledgeGraph.ConceptClassificationValue)

                'Dim lsOldConceptClassificationType = Me.msOldValue

                '' Update the ClassificationType property of the ConceptClassificationValue object
                'lrConceptClassification.ClassificationType = lsUpdatedClassificationType

                '' Perform the upsert operation with whereClause based on the old ClassificationType
                'Dim lrDataStore As New DataStore.Store
                'Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) =
                'Function(p) p.ModelId = Me.mrModelElement.Model.ModelId And p.Concept = lrConceptClassification.Concept And p.ClassificationType = lsOldConceptClassificationType

                'If lrConceptClassification.Concept <> "" _
                '    And lrConceptClassification.ClassificationType <> "" _
                '    And lrConceptClassification.ClassificationValue <> "" Then

                '    lrDataStore.Upsert(lrConceptClassification, whereClause)
                'End If

                'Me.msNewValue = lsUpdatedClassificationType

                '' Refresh the DataGridView to reflect the changes
                DataGridView.Refresh()
#End Region
                ElseIf e.ColumnIndex > 2 Then ' Assuming ClassificationValue column starts at index 3 (0-based index)

                    ' Get the updated ClassificationValue value from the cell
                    Dim lsUpdatedClassificationValue As String = DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()

                ' Get the associated ConceptClassificationValue object from the BindingList
                Dim lrConceptClassification As KnowledgeGraph.ConceptClassificationValue = CType(DataGridView.Rows(e.RowIndex).DataBoundItem, KnowledgeGraph.ConceptClassificationValue)

                Dim lsOldConceptClassificationValue = Me.msOldValue

                ' Update the ClassificationValue property of the ConceptClassificationValue object
                lrConceptClassification.ClassificationValue = lsUpdatedClassificationValue

                ' Perform the upsert operation with whereClause based on the old ClassificationType and ClassificationValue
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) =
                    Function(p) p.ModelId = Me.mrModelElement.Model.ModelId And p.Concept = lrConceptClassification.Concept And p.ClassificationType = lrConceptClassification.ClassificationType

                If lrConceptClassification.Concept <> "" _
                    And lrConceptClassification.ClassificationType <> "" _
                    And lrConceptClassification.ClassificationValue <> "" Then

                    lrDataStore.Upsert(lrConceptClassification, whereClause)
                End If
                Me.msNewValue = lsUpdatedClassificationValue


                ' Refresh the DataGridView to reflect the changes
                DataGridView.Refresh()
                End If

                BeginInvoke(Sub()
                            ' Create a new TextBox cell and set its value
                            Dim textBoxCell As New DataGridViewTextBoxCell()
                            textBoxCell.Value = Me.msNewValue

                            ' Replace the ComboBox cell with the TextBox cell
                            Me.DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex) = textBoxCell
                        End Sub)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView.DataError

        Try
            Call prApplication.ThrowErrorMessage("Check field values", pcenumErrorType.Warning, abThrowtoMSGBox:=True, abUseFlashCard:=True, abSuppressLogging:=True)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView.UserDeletedRow

        Try
            'CodeSafe
            If Me.mrSelectedRowDataItem Is Nothing Then Exit Sub

            Dim lrConceptClassification As KnowledgeGraph.ConceptClassificationValue = Me.mrSelectedRowDataItem

            Dim lrDataStore As New DataStore.Store
            Dim loWhereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) =
                    Function(p) p.ModelId = lrConceptClassification.ModelId And p.Concept = lrConceptClassification.Concept And p.ClassificationType = lrConceptClassification.ClassificationType
            Call lrDataStore.Delete(Of KnowledgeGraph.ConceptClassificationValue)(loWhereClause)

            Me.mrSelectedRowDataItem = Nothing

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


End Class