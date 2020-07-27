Public Class frmFactEngine

    Public zrScanner As New FEQL.Scanner
    Public zrParser As New FEQL.Parser(Me.zrScanner)
    Public WithEvents zrTextHighlighter As FEQL.TextHighlighter

    Public FEQLProcessor As New FEQL.Processor(prApplication.WorkingModel)

    Private Sub frmFactEngine_Load(sender As Object, e As EventArgs) Handles Me.Load

        If prApplication.WorkingModel Is Nothing Then
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: Select a Model in the Model Explorer"
        Else
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: " & prApplication.WorkingModel.Name
        End If

        '-------------------------------------------------------
        'Setup the Text Highlighter
        '----------------------------
        Me.zrTextHighlighter = New FEQL.TextHighlighter(
                               Me.TextBoxInput,
                               Me.zrScanner,
                               Me.zrParser)


    End Sub

    Private Sub ToolStripButtonGO_Click(sender As Object, e As EventArgs) Handles ToolStripButtonGO.Click

        Dim lrRecordset As New ORMQL.Recordset

        lrRecordset = Me.FEQLProcessor.ProcessFEQLStatement(Me.TextBoxInput.Text)

        If lrRecordset.ErrorString IsNot Nothing Then
            Me.LabelError.BringToFront()
            Me.LabelError.Text = lrRecordset.ErrorString
        Else
            'ToDo
        End If

    End Sub

    Private Sub TextBoxInput_TextChanged(sender As Object, e As EventArgs) Handles TextBoxInput.TextChanged

    End Sub
End Class