Public Class frmToolboxTableData

    Public mrModel As FBM.Model
    Public mrTableName As String = Nothing
    Private mrRecordset As ORMQL.Recordset

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
            If mrTableName Is Nothing Then
            Else
                lsSQLQuery = "SELECT * FROM " & mrTableName
                Me.mrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsSQLQuery)

                Dim lrDataGridList As New ORMQL.RecordsetDataGridList(Me.mrRecordset)
                Me.DataGridView.DataSource = lrDataGridList

            End If
        End If

    End Sub

    Private Sub frmToolboxTableData_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub DataGridView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView.CellEndEdit

        'Dim lsValue = Me.DataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        'Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data = lsValue

    End Sub

    Private Sub DataGridView_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridView.CellValidating


        'Debugger.Break()
        Me.mrRecordset.Facts(e.RowIndex)(Me.mrRecordset.Columns(e.ColumnIndex)).Data = e.FormattedValue
    End Sub
End Class