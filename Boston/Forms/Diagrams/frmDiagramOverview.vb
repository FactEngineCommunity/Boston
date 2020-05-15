Public Class frmDiagramOverview

    Public Sub SetDocument(ByVal aMFView_view As MindFusion.Diagramming.WinForms.DiagramView)

        Overview.DiagramView = aMFView_view
        Overview.FitAll = False
        Overview.FitAll = True

    End Sub


End Class