Public Class frmLicences

    Private Sub frmLicences_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim lsApplicationPath As String = Boston.MyPath

        Me.WebBrowser.Navigate("file:\\" & lsApplicationPath & "\licenses\index.html")

    End Sub

End Class