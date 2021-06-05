Public Class frmToolboxTableData

    Public mrModel As FBM.Model
    Public mrTable As RDS.Table = Nothing
    Private mrRecordset As ORMQL.Recordset

    'For Cell text editting
    Private OldValue, NewValue As String

    Public Function EqualsByName(ByVal other As Form) As Boolean
        Return Me.Name = other.Name
    End Function

    Private Sub frmToolboxTableData_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm

    End Sub

    Public Sub SetupForm()

        Dim lsSQLQuery As String

        If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
        Else
            If Me.mrTable Is Nothing Then
            Else
                lsSQLQuery = "SELECT * FROM " & mrTable.Name
                Me.mrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsSQLQuery)


                Dim lrDataGridList As New ORMQL.RecordsetDataGridList(Me.mrRecordset, Me.mrTable)
                Me.DataGridView.DataSource = lrDataGridList

            End If
        End If

    End Sub

    Private Sub frmToolboxTableData_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub DataGridView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView.CellEndEdit

        Try
            Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data = Me.NewValue

            Dim lrColumn As RDS.Column = Me.mrTable.Column.Find(Function(x) x.Name = Me.mrRecordset.Columns(e.ColumnIndex))

            Dim larPKColumn As List(Of RDS.Column) = Me.mrTable.getPrimaryKeyColumns

            Dim liColumnIndex As Integer
            Dim lsValue As String
            For Each lrPKColumn In larPKColumn
                liColumnIndex = Me.mrRecordset.Columns.IndexOf(lrPKColumn.Name)
                lsValue = Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(liColumnIndex)).Data
                lrPKColumn.TemporaryData = lsValue
            Next

            Call prApplication.WorkingModel.DatabaseConnection.UpdateAttributeValue(Me.mrTable.Name, lrColumn, Me.NewValue, larPKColumn)

        Catch ex As Exception

        End Try

    End Sub

    Private Sub DataGridView_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView.CellBeginEdit

        Me.OldValue = Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data

    End Sub

    Private Sub DataGridView_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridView.CellValidating

        Me.NewValue = e.FormattedValue

    End Sub
End Class