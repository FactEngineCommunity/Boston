Public Class frmSignatueResolution

    Private cManager As CurrencyManager
    Public zarSignatureResolutionList As New List(Of Object)

    Private Sub frmSignatueResolution_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Call Me.PopulateDataGridWithSignatureResolutionItems()

    End Sub

    Sub PopulateDataGridWithSignatureResolutionItems()

        '---------------------------------
        'Bind the tuples to the DataGrid
        '---------------------------------       
        DataGridView.DataSource = Nothing
        DataGridView.DataSource = zarSignatureResolutionList

        cManager = CType(DataGridView.BindingContext(zarSignatureResolutionList), CurrencyManager)

        Me.DataGridView.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridView.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells


    End Sub

End Class