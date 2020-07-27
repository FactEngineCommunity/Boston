Public Class frmFactEngine

    Public zrScanner As New FEQL.Scanner
    Public zrParser As New FEQL.Parser(Me.zrScanner)
    Public WithEvents zrTextHighlighter As FEQL.TextHighlighter
    Public WithEvents Application As tRichmondApplication = prApplication

    Public FEQLProcessor As New FEQL.Processor(prApplication.WorkingModel)

    Private Sub displayModelName()
        If prApplication.WorkingModel Is Nothing Then
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: Select a Model in the Model Explorer"
        Else
            Me.ToolStripStatusLabelWorkingModelName.Text = "Model: " & prApplication.WorkingModel.Name
        End If
    End Sub

    Private Sub frmFactEngine_Load(sender As Object, e As EventArgs) Handles Me.Load

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
            Me.LabelError.Text = ""

            For Each lrFact In lrRecordset.Facts
                Me.LabelError.Text = lrFact.EnumerateAsBracketedFact & vbCrLf
            Next

            'ToDo
        End If

    End Sub

    Private Sub Application_WorkingModelChanged() Handles Application.WorkingModelChanged
        Call Me.displayModelName()
        Me.FEQLProcessor = New FEQL.Processor(prApplication.WorkingModel)
    End Sub
End Class