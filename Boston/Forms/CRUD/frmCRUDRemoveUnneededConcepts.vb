Public Class frmCRUDRemoveUnneededConcepts

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub ButtonRemoveUnneededConcepts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemoveUnneededConcepts.Click

        frmMain.Cursor = Cursors.WaitCursor

        Call TableConcept.DeleteUnneededConcepts()
        Me.LabelUnneededConceptsRemoved.Visible = True
        Me.ButtonRemoveUnneededConcepts.Enabled = False

        frmMain.Cursor = Cursors.Default

    End Sub
End Class