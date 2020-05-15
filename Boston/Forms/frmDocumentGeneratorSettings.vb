Public Class frmDocumentGeneratorSettings

    Public zrORMWordVerbaliser As FBM.ORMWordVerbailser


    Private Sub ButtonGenerateDocumentation_Click(sender As Object, e As EventArgs) Handles ButtonGenerateDocumentation.Click

        zrORMWordVerbaliser.PutPageDiagramsAtEndOfDocument = CheckBoxDisplayPageImages.Checked
        zrORMWordVerbaliser.IncludePagesOfSameNameAsModelElements = CheckBoxDisplayTermDiagram.Checked

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

End Class