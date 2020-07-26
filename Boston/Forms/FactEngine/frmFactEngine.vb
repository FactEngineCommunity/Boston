Public Class frmFactEngine

    Private Sub frmFactEngine_Load(sender As Object, e As EventArgs) Handles Me.Load

        If prApplication.WorkingModel Is Nothing Then
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: Select a Model in the Model Explorer"
        Else
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: " & prApplication.WorkingModel.Name
        End If

    End Sub

End Class