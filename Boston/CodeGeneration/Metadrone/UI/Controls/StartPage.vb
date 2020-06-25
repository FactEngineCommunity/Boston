Namespace UI

    Friend Class StartPage

        Private WithEvents recents As New List(Of StartPageRecent)

        Public Event NewProject()
        Public Event OpenProject()
        Public Event OpenRecent(ByVal path As String)

        Private Sub StartPage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Init descriptions
            Me.lblBuild.Text = Globals.ASSEMBLY_VERSION_METADRONE
        End Sub

        Public Sub OpenRecentProject(ByVal path As String)
            RaiseEvent OpenRecent(path)
        End Sub

        Public Sub LoadRecents(ByVal settings As Persistence.Settings.Settings)
            Me.pnlRecents.Controls.Clear()
            For Each rct In settings.RecentProjects
                Dim rc As New StartPageRecent(rct.ProjectName, rct.Path)
                rc.Dock = DockStyle.Top
                AddHandler rc.OpenRecent, AddressOf OpenRecentProject
                Me.pnlRecents.Controls.Add(rc)
            Next
        End Sub

        Private Sub lnkNewProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkNewProject.Click
            RaiseEvent NewProject()
        End Sub

        Private Sub lnkOpenProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkOpenProject.Click
            RaiseEvent OpenProject()
        End Sub

        Private Sub lnkAbout_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkAbout.LinkClicked
            Dim frm As New About()
            frm.ShowDialog(Me)
        End Sub

        Private Sub lnkAbout_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAbout.MouseEnter
            Me.lnkAbout.LinkColor = Color.Black
        End Sub

        Private Sub lnkAbout_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAbout.MouseLeave
            Me.lnkAbout.LinkColor = Color.FromArgb(85, 85, 85)
        End Sub

    End Class

End Namespace