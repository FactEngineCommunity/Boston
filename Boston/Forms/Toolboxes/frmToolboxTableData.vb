Public Class frmToolboxTableData

    Public mrModel As FBM.Model
    Public mrTableName As String = Nothing

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
                Dim lrRecordset = prApplication.WorkingModel.DatabaseConnection.GO(lsSQLQuery)

                Dim lrDataGridList As New ORMQL.RecordsetDataGridList(lrRecordset)
                Me.DataGridView.DataSource = lrDataGridList

            End If
        End If

    End Sub

    Private Sub frmToolboxTableData_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

End Class