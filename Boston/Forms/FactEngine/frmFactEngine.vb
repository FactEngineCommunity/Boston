Public Class frmFactEngine

    Public zrScanner As FEQL.Scanner
    Public zrParser As FEQL.Parser
    Public WithEvents zrTextHighlighter As FEQL.TextHighlighter

    Private Sub frmFactEngine_Load(sender As Object, e As EventArgs) Handles Me.Load

        If prApplication.WorkingModel Is Nothing Then
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: Select a Model in the Model Explorer"
        Else
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: " & prApplication.WorkingModel.Name
        End If

        '-------------------------------------------------------
        'Setup the Parser etc
        '---------------------
        zrScanner = New FEQL.Scanner
        zrParser = New FEQL.Parser(zrScanner)

        Me.zrTextHighlighter = New FEQL.TextHighlighter(
                               Me.TextBoxInput,
                               Me.zrScanner,
                               Me.zrParser)


    End Sub

End Class