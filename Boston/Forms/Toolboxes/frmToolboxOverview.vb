Public Class frmToolboxOverview

    Private Sub overview_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


    End Sub

    Public Sub SetDocument(ByVal aMFView_view As MindFusion.Diagramming.WinForms.DiagramView)

        Overview.DiagramView = aMFView_view
        Overview.FitAll = False
        Overview.FitAll = True

    End Sub

    Private Sub Overview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Overview.Click

    End Sub

End Class