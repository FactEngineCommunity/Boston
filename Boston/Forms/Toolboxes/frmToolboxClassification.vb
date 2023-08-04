Imports System.Reflection
Imports System.Linq.Expressions
Imports System.ComponentModel

Public Class frmToolboxConceptClassification

    Public WithEvents mrModelElement As FBM.ModelObject
    Private mrBindingList As BindingList(Of KnowledgeGraph.ConceptClassificationValue)
    Private msOldClassificationType As String = ""

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmToolboxClassification_Load(sender As Object, e As EventArgs) Handles Me.Load


    End Sub

    Public Sub SetupForm(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try
            Me.mrModelElement = arModelElement

            If arModelElement Is Nothing Then
                Me.DataGridView1.DataSource = Nothing
                Me.LabelModelElementName.Text = "<Nothing Selected>"
            Else
                Me.mrBindingList = New BindingList(Of KnowledgeGraph.ConceptClassificationValue)(Me.mrModelElement.ClassificationValue)
                Me.DataGridView1.DataSource = Me.mrBindingList
                Me.LabelModelElementName.Text = Me.mrModelElement.Id
            End If

            Me.DataGridView1.ReadOnly = False

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

            ' Create a new object of type KnowledgeGraph.ClassificationValue
            Dim newClassificationValue As New KnowledgeGraph.ConceptClassificationValue(Me.mrModelElement.Model, Me.mrModelElement, "", "")

            ' Add the new object to the list
            Me.mrModelElement.ClassificationValue.Add(newClassificationValue)

            '=====================================================================
            'Data Store update
            Dim lrConceptClassificationValue As New KnowledgeGraph.ConceptClassificationValue(Me.mrModelElement.Model, Me.mrModelElement, "", "")
            Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) = Function(p) p.Concept = newClassificationValue.Concept And p.ClassificationType = newClassificationValue.ClassificationType
            Dim lrDataStore As New DataStore.Store
            Call lrDataStore.Upsert(lrConceptClassificationValue, whereClause)
            '=====================================================================

            Me.mrBindingList.ResetBindings()


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_UserAddedRow(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView1.UserAddedRow

        Try
            'CodeSafe
            If Me.mrModelElement Is Nothing Then Exit Sub

            ' Create a new object of type KnowledgeGraph.ClassificationValue
            Dim lrConceptClassificationValue As New KnowledgeGraph.ConceptClassificationValue(Me.mrModelElement.Model, Me.mrModelElement, "", "")

            ' Set the properties of the new object based on the DataGridView input
            lrConceptClassificationValue.Model = Me.mrModelElement.Model

            ' Add the new object to the list
            Me.mrModelElement.ClassificationValue.Add(lrConceptClassificationValue)

            '=====================================================================
            'Data Store update
            Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) = Function(p) p.Concept = lrConceptClassificationValue.Concept And p.ClassificationType = lrConceptClassificationValue.ClassificationType
            Dim lrDataStore As New DataStore.Store
            Call lrDataStore.Upsert(lrConceptClassificationValue, whereClause)
            '=====================================================================

            Me.mrBindingList.ResetBindings()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded

        Try
            ' Check if the DataGridView is in edit mode
            If DataGridView1.IsCurrentCellInEditMode Then
                ' Cancel the edit mode before adding a new row
                DataGridView1.EndEdit()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub DataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit
        Try
            ' Check if the edited cell is in the ClassificationType column (second column)
            If e.ColumnIndex = 1 Then ' Assuming ClassificationType column is at index 1 (0-based index)

                ' Get the associated ConceptClassificationValue object from the BindingList
                Dim conceptClassificationValue As KnowledgeGraph.ConceptClassificationValue = CType(DataGridView1.Rows(e.RowIndex).DataBoundItem, KnowledgeGraph.ConceptClassificationValue)

                ' Store the old ClassificationType value before editing starts
                Me.msOldClassificationType = conceptClassificationValue.ClassificationType
            End If
        Catch ex As Exception
            ' Handle the exception
            ' ...
        End Try
    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit

        Try
            ' Check if the edited cell is in the ClassificationType column (second column)
            If e.ColumnIndex = 1 Then ' Assuming ClassificationType column is at index 1 (0-based index)

                ' Get the updated ClassificationType value from the cell
                Dim updatedClassificationType As String = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()

                ' Get the associated ConceptClassificationValue object from the BindingList
                Dim lrConceptClassificationValue As KnowledgeGraph.ConceptClassificationValue = CType(DataGridView1.Rows(e.RowIndex).DataBoundItem, KnowledgeGraph.ConceptClassificationValue)

                Dim lsOldConceptClassificationType = lrConceptClassificationValue.ClassificationType

                ' Update the ClassificationType property of the ConceptClassificationValue object
                lrConceptClassificationValue.ClassificationType = updatedClassificationType

                ' Perform the upsert operation with whereClause based on the old ClassificationType
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) =
                Function(p) p.Concept = lrConceptClassificationValue.Concept And p.ClassificationType = Me.msOldClassificationType

                lrDataStore.Upsert(lrConceptClassificationValue, whereClause)


                ' Refresh the DataGridView to reflect the changes
                DataGridView1.Refresh()
            ElseIf e.ColumnIndex > 1 Then ' Assuming ClassificationValue column starts at index 2 (0-based index)

                ' Get the updated ClassificationValue value from the cell
                Dim updatedClassificationValue As String = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()

                ' Get the associated ConceptClassificationValue object from the BindingList
                Dim lrConceptClassificationValue As KnowledgeGraph.ConceptClassificationValue = CType(DataGridView1.Rows(e.RowIndex).DataBoundItem, KnowledgeGraph.ConceptClassificationValue)

                ' Update the ClassificationValue property of the ConceptClassificationValue object
                lrConceptClassificationValue.ClassificationValue = updatedClassificationValue

                ' Perform the upsert operation with whereClause based on the old ClassificationType and ClassificationValue
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of KnowledgeGraph.ConceptClassificationValue, Boolean)) =
                    Function(p) p.Concept = lrConceptClassificationValue.Concept And p.ClassificationType = lrConceptClassificationValue.ClassificationType

                lrDataStore.Upsert(lrConceptClassificationValue, whereClause)

                ' Refresh the DataGridView to reflect the changes
                DataGridView1.Refresh()
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